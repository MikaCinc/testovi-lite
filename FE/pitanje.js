import { getApiURL } from "./common.js";

class Pitanje {
  constructor(pitanje) {
    this.question = pitanje;
  }

  handleQuestionDelete = (id) => {
    fetch(getApiURL() + "Pitanje/IzbrisiPitanje/" + id, {
      method: "delete",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
    })
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
        /* this.state = {
          ...this.state,
          pitanja: [...this.state.pitanja.filter((p) => p.id !== id)],
        };
        console.log(this.state);
        this.render(); */
        document.getElementById("questionContainer-" + id).remove();
      })
      .catch((err) => {});
  };

  render() {
    const questionsContainer = document.querySelector(".questionsContainer");
    const questionElement = document.createElement("div");
    const questionP = document.createElement("p");
    questionP.className = "questionText";
    questionP.innerHTML = this.question.question;

    const answerInput = document.createElement("input");
    answerInput.className = "answerInput";
    answerInput.setAttribute("type", "text");
    answerInput.setAttribute("placeholder", "Odgovor?");

    const checkButton = document.createElement("button");
    checkButton.className = "button checkButton";
    checkButton.innerHTML = "Proveri";
    checkButton.addEventListener("click", () => {
      if (
        answerInput.value.toLowerCase() === this.question.answer.toLowerCase()
      ) {
        alert("Odgovor je tačan!");
      } else {
        alert("Netačno!");
      }
    });

    const showButton = document.createElement("button");
    showButton.className = "button showButton";
    showButton.innerHTML = "👁️‍🗨️Prikaži";
    showButton.addEventListener("click", () => {
      answerInput.value = this.question.answer;
    });

    const actions = document.createElement("div");
    actions.className = "singleQuestionActions";
    actions.appendChild(checkButton);
    actions.appendChild(showButton);

    const deleteBtn = document.createElement("button");
    deleteBtn.innerHTML = "🗑️";
    deleteBtn.className = "button deleteQuestionBtn";
    deleteBtn.addEventListener("click", () =>
      this.handleQuestionDelete(this.question.id)
    );

    questionElement.className = "singleQuestionContainer";
    questionElement.id = "questionContainer-" + this.question.id;
    questionElement.appendChild(questionP);
    questionElement.appendChild(answerInput);
    questionElement.appendChild(deleteBtn);
    questionElement.appendChild(actions);
    questionsContainer.appendChild(questionElement);
  }
}

export default Pitanje;
