import { getApiURL, fetchAllDataInSet } from "./common.js";
import Pitanje from "./pitanje.js";
import Spojnica from "./spojnica.js";
import state from "./index.js";

class Home {
  constructor(data) {
    this.state = {
      ...data,
    };

    this.currentSet = data.setovi[0].id;

    this.handleNewTag = this.handleNewTag.bind(this);
    // this.handleTagDelete = this.handleTagDelete.bind(this);
    this.handleNewQuestion = this.handleNewQuestion.bind(this);
    this.render = this.render.bind(this);
  }

  handleNewTag() {
    const inputElement = document.querySelector(".newTagInput");
    const newTag = inputElement.value;
    inputElement.value = "";

    fetch(getApiURL() + "Tag/DodajTag/" + this.currentSet, {
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

    fetch(getApiURL() + "Pitanje/DodajPitanje/" + this.currentSet, {
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

  fetchSpojniceByTag = (tagId) => {
    fetch(getApiURL() + "Search/PreuzmiSpojnicePoTagu/" + tagId)
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        this.state = {
          ...this.state,
          spojnice: data,
        };
        this.render();
      })
      .catch((err) => {});
  };

  fetchSpojniceByString = () => {
    let value = document.querySelector("#searchInput").value;
    if (value.length < 3) return;

    fetch(getApiURL() + "Search/PreuzmiSpojnicePoNazivu/" + value)
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        this.state = {
          ...this.state,
          spojnice: data,
        };
        this.render();
      })
      .catch((err) => {});
  };

  renderSettings() {
    const container = document.querySelector(".homeContentContainer");
    const settingsContainer = document.createElement("div");
    settingsContainer.className = "settingsContainer";

    const title = document.createElement("h3");
    title.innerHTML = "Podešavanja";
    settingsContainer.appendChild(title);

    const primaryP = document.createElement("p");
    primaryP.innerHTML = "Primary boja:";
    primaryP.style.color = "var(--primary)";
    settingsContainer.appendChild(primaryP);

    const primary = document.createElement("input");
    primary.className = "primaryColorInput";
    primary.setAttribute("type", "color");
    primary.setAttribute(
      "value",
      getComputedStyle(document.documentElement).getPropertyValue("--primary")
    );
    primary.addEventListener("change", () => {
      document.documentElement.style.setProperty("--primary", primary.value);
    });
    settingsContainer.appendChild(primary);

    const secondaryP = document.createElement("p");
    secondaryP.innerHTML = "Secondary boja:";
    secondaryP.style.color = "var(--secondary)";
    settingsContainer.appendChild(secondaryP);

    const secondary = document.createElement("input");
    secondary.className = "secondaryColorInput";
    secondary.setAttribute("type", "color");
    secondary.setAttribute(
      "value",
      getComputedStyle(document.documentElement).getPropertyValue("--secondary")
    );
    secondary.addEventListener("change", () => {
      document.documentElement.style.setProperty(
        "--secondary",
        secondary.value
      );
    });
    settingsContainer.appendChild(secondary);

    const tagP = document.createElement("p");
    tagP.innerHTML = "Boja tagova:";
    tagP.style.color = "var(--tagColor)";
    settingsContainer.appendChild(tagP);

    const tag = document.createElement("input");
    tag.className = "tagColorInput";
    tag.setAttribute("type", "color");
    tag.setAttribute(
      "value",
      getComputedStyle(document.documentElement).getPropertyValue("--tagColor")
    );
    tag.addEventListener("change", () => {
      document.documentElement.style.setProperty("--tagColor", tag.value);
    });
    settingsContainer.appendChild(tag);

    container.appendChild(settingsContainer);
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
      tagP.onclick = () => {
        this.fetchSpojniceByTag(tag.id);
      };
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
    submitButton.innerHTML = "➕ Dodaj pitanje";
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

    const actions = document.createElement("div");
    actions.className = "singleQuestionActions";

    const newBtn = document.createElement("button");
    newBtn.className = "button newSpojnicaBtn";
    newBtn.innerHTML = "🆕 Nova spojnica";
    newBtn.style.marginBottom = "15px";
    newBtn.addEventListener("click", () => {
      const newSpojnica = new Spojnica();
      newSpojnica.novaSpojnica(this.currentSet);
    });

    actions.appendChild(newBtn);

    /* Sort button */
    const sortBtn = document.createElement("button");
    sortBtn.className = "button sortSpojnicaBtn";
    sortBtn.innerHTML = "🔻";
    sortBtn.style.marginBottom = "15px";
    sortBtn.addEventListener("click", () => {
      this.state.spojnice.sort((a, b) => {
        return a.priority - b.priority;
      });
      this.render();
    });

    actions.appendChild(sortBtn);
    spojniceContainer.appendChild(actions);

    /* Spojnice iz state-a */
    const spojnice = this.state.spojnice;
    if (!spojnice || !spojnice.length) return;
    spojnice.forEach((spojnica) => {
      const s = new Spojnica(spojnica, this.render);
      s.renderTile(); // Posebna render metoda
    });
  }

  handleSetChange = (setId) => {
    this.currentSet = setId;
    fetchAllDataInSet(setId, ({ tagovi, pitanja, spojnice }) => {
      this.state.tagovi = tagovi;
      this.state.pitanja = pitanja;
      this.state.spojnice = spojnice;
      /* Azuriramo i globalni state jer se on koristi unutar spojnica */
      state.tagovi = tagovi;
      state.pitanja = pitanja;
      state.spojnice = spojnice;

      this.render();
    });
  };

  renderSets = () => {
    let container = document.querySelector(".homeContainer");
    let setsContainer = document.createElement("div");
    setsContainer.className = "setsContainer";

    const sets = this.state.setovi;
    sets.forEach((set) => {
      const setElement = document.createElement("div");
      const isSelected = this.currentSet === set.id;
      setElement.className = `setButton button ${
        isSelected ? "correctQuestion" : ""
      }`;
      setElement.innerHTML = (isSelected ? "" : "🟠") + set.title;
      setElement.onclick = () => {
        this.handleSetChange(set.id);
      };
      setsContainer.appendChild(setElement);
    });

    container.appendChild(setsContainer);
  };

  render() {
    let container = document.querySelector(".homeContainer");
    if (!container) {
      container = document.createElement("div");
    }
    document.body.appendChild(container);
    container.innerHTML = "";
    container.className = "homeContainer";

    const contentContainer = document.createElement("div");
    contentContainer.className = "homeContentContainer";

    const searchContainer = document.createElement("div");
    searchContainer.className = "searchContainer";

    const searchInput = document.createElement("input");
    searchInput.id = "searchInput";
    searchInput.setAttribute("type", "text");
    searchInput.setAttribute("placeholder", "Pretraži pitanja i spojnice");

    const submitButton = document.createElement("button");
    submitButton.className = "button submitButton";
    submitButton.innerHTML = "🔎 Pretraži";
    submitButton.addEventListener("click", this.fetchSpojniceByString);

    searchContainer.appendChild(searchInput);
    searchContainer.appendChild(submitButton);
    container.appendChild(searchContainer);
    this.renderSets();
    container.appendChild(contentContainer);

    this.renderSpojnice();
    this.renderQuestions();
    this.renderTags();
    this.renderSettings();
  }
}

export default Home;
