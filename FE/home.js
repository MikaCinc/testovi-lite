import { getApiURL } from "./common.js";

class Home {
  constructor(data) {
    this.state = {
      ...data,
    };

    this.handleNewTag = this.handleNewTag.bind(this);
    // this.handleTagDelete = this.handleTagDelete.bind(this);
  }

  handleNewTag() {
    const inputElement = document.querySelector(".newTagInput");
    const newTag = inputElement.value;
    inputElement.value = "";

    fetch(getApiURL() + "Tag/DodajTag", {
      method: "post",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
      body: JSON.stringify({
        title: newTag,
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        if (data?.id) {
          this.state = {
            ...this.state,
            tagovi: [...this.state.tagovi, data],
          };
          console.log(this.state);
          this.render();
        }
      })
      .catch((err) => {});
  }

  handleTagDelete = (id) => {
    fetch(getApiURL() + "Tag/IzbrisiTag/" + id, {
      method: "delete",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        this.state = {
          ...this.state,
          tagovi: [...this.state.tagovi.filter((tag) => tag.id !== id)],
        };
        console.log(this.state);
        this.render();
      })
      .catch((err) => {});
  };

  renderTags() {
    const container = document.querySelector(".homeContentContainer");
    const tagsContainer = document.createElement("div");
    tagsContainer.className = "tagsContainer";

    const tags = this.state.tagovi;
    tags.forEach((tag) => {
      const tagElement = document.createElement("div");
      const tagP = document.createElement("p");
      const deleteBtn = document.createElement("button");
      deleteBtn.innerHTML = "X";
      deleteBtn.className = "button deleteBtn";
      deleteBtn.addEventListener("click", () => this.handleTagDelete(tag.id));
      tagP.className = "tag";
      tagP.innerHTML = tag.title;
      tagElement.className = "singleTagContainer";
      tagElement.appendChild(tagP);
      tagElement.appendChild(deleteBtn);
      tagsContainer.appendChild(tagElement);
    });

    const newTagInput = document.createElement("input");
    newTagInput.className = "newTagInput";
    newTagInput.setAttribute("type", "text");
    newTagInput.setAttribute("placeholder", "Novi tag...");
    const submitButton = document.createElement("button");
    submitButton.className = "button newTagButton";
    submitButton.innerHTML = "Dodaj tag";
    submitButton.addEventListener("click", this.handleNewTag);

    tagsContainer.appendChild(newTagInput);
    tagsContainer.appendChild(submitButton);

    container.appendChild(tagsContainer);
  }

  render() {
    let container = document.querySelector(".homeContainer");
    if (!container) {
      container = document.createElement("div");
    }
    container.innerHTML = "";
    container.className = "homeContainer";

    const contentContainer = document.createElement("div");
    contentContainer.className = "homeContentContainer";

    const searchContainer = document.createElement("div");
    searchContainer.className = "searchContainer";

    const searchInput = document.createElement("input");
    searchInput.className = "searchInput";
    searchInput.setAttribute("type", "text");
    searchInput.setAttribute("placeholder", "Pretraži pitanja i spojnice");

    const submitButton = document.createElement("button");
    submitButton.className = "button submitButton";
    submitButton.innerHTML = "Pretraži";

    searchContainer.appendChild(searchInput);
    searchContainer.appendChild(submitButton);
    container.appendChild(searchContainer);
    container.appendChild(contentContainer);
    document.body.appendChild(container);

    this.renderTags();
  }
}

export default Home;
