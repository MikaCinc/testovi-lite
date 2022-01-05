import { getApiURL } from "./common.js";
import Pitanje from "./pitanje.js";
import Spojnica from "./spojnica.js";
import state from "./index.js";

class Home {
  constructor(data) {
    this.state = {
      ...data,
    };

    this.handleNewTag = this.handleNewTag.bind(this);
    // this.handleTagDelete = this.handleTagDelete.bind(this);
    this.handleNewQuestion = this.handleNewQuestion.bind(this);
    this.render = this.render.bind(this);
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
          /* state = {
            // Global state
            ...state,
            tagovi: [...state.tagovi, data],
          };
          console.log(this.state, state); */
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
          spojnice: [
            ...this.state.spojnice.map((spojnica) => ({
              ...spojnica,
              tagovi: spojnica.tagovi.filter((tag) => tag.id !== id),
            })),
          ],
        };
        console.log(this.state);
        this.render();
      })
      .catch((err) => {});
  };

  handleNewQuestion() {
    const inputElement = document.querySelector(".newQuestionInput");
    const newQuestion = inputElement.value;
    inputElement.value = "";
    const inputElementAnswer = document.querySelector(".newAnswerInput");
    const newAnswer = inputElementAnswer.value;
    inputElementAnswer.value = "";

    fetch(getApiURL() + "Pitanje/DodajPitanje", {
      method: "post",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
      body: JSON.stringify({
        question: newQuestion,
        answer: newAnswer,
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        if (data?.id) {
          this.state = {
            ...this.state,
            pitanja: [data, ...this.state.pitanja],
          };
          console.log(this.state);
          this.render();
        }
      })
      .catch((err) => {});
  }

  renderTags() {
    const container = document.querySelector(".homeContentContainer");
    const tagsContainer = document.createElement("div");
    tagsContainer.className = "tagsContainer";

    const title = document.createElement("h3");
    title.innerHTML = "Tagovi";
    tagsContainer.appendChild(title);

    const tags = this.state.tagovi;
    tags.forEach((tag) => {
      const tagElement = document.createElement("div");
      const tagP = document.createElement("p");
      const deleteBtn = document.createElement("button");
      deleteBtn.innerHTML = "🗑️";
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
    submitButton.innerHTML = "➕ Dodaj tag";
    submitButton.addEventListener("click", this.handleNewTag);

    tagsContainer.appendChild(newTagInput);
    tagsContainer.appendChild(submitButton);

    container.appendChild(tagsContainer);
  }

  renderQuestions() {
    const container = document.querySelector(".homeContentContainer");
    const questionsContainer = document.createElement("div");
    questionsContainer.className = "questionsContainer";
    container.appendChild(questionsContainer);

    /* Naslov sekcije */
    const title = document.createElement("h3");
    title.innerHTML = "Pitanja";
    questionsContainer.appendChild(title);

    /* Novo pitanje */
    const questionElement = document.createElement("div");
    questionElement.className = "singleQuestionContainer";

    const newQuestionInput = document.createElement("input");
    newQuestionInput.className = "newQuestionInput";
    newQuestionInput.setAttribute("type", "text");
    newQuestionInput.setAttribute("placeholder", "Novo pitanje...");
    const newAnswerInput = document.createElement("input");
    newAnswerInput.className = "newAnswerInput";
    newAnswerInput.setAttribute("type", "text");
    newAnswerInput.setAttribute("placeholder", "Novi odgovor...");

    const actions = document.createElement("div");
    actions.className = "singleQuestionActions";

    const submitButton = document.createElement("button");
    submitButton.className = "button newQuestionButton";
    submitButton.innerHTML = "Dodaj pitanje";
    submitButton.addEventListener("click", this.handleNewQuestion);

    actions.appendChild(submitButton);
    questionElement.appendChild(newQuestionInput);
    questionElement.appendChild(newAnswerInput);
    questionElement.appendChild(actions);
    questionsContainer.appendChild(questionElement);

    /* Pitanja iz state-a */
    const questions = this.state.pitanja;
    questions.forEach((question) => {
      const pitanje = new Pitanje(question);
      pitanje.render();
    });
  }

  renderSpojnice() {
    const container = document.querySelector(".homeContentContainer");
    const spojniceContainer = document.createElement("div");
    spojniceContainer.className = "spojniceContainer";
    container.appendChild(spojniceContainer);

    /* Naslov sekcije */
    const title = document.createElement("h3");
    title.innerHTML = "Spojnice";
    spojniceContainer.appendChild(title);

    /* Nova spojnica */
    const newBtn = document.createElement("button");
    newBtn.className = "button newSpojnicaBtn";
    newBtn.innerHTML = "🆕 Nova spojnica";
    newBtn.addEventListener(
      "click",
      () => {
        const newSpojnica = new Spojnica();
        newSpojnica.novaSpojnica();
      } /* TODO */
    );

    spojniceContainer.appendChild(newBtn);

    /* Spojnice iz state-a */
    const spojnice = this.state.spojnice;
    spojnice.forEach((spojnica) => {
      const s = new Spojnica(spojnica, this.render);
      s.renderTile(); // Posebna render metoda
    });
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

    this.renderSpojnice();
    this.renderQuestions();
    this.renderTags();
  }
}

export default Home;
