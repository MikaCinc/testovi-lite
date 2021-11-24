const mockSpojnica = {
  id: "s-1",
  title: "Glavni gradovi",
  questions: [
    {
      id: "1",
      question: "Srbija",
      answer: "Beograd",
    },
    {
      id: "2",
      question: "Španija",
      answer: "Madrid",
    },
    {
      id: "3",
      question: "Grčka",
      answer: "Atina",
    },
  ],
};

window.onload = () => {
  const spojnica = new Spojnica(mockSpojnica);
  spojnica.render();
};

class Spojnica {
  constructor(spojnica) {
    this.id = spojnica.id;
    this.title = spojnica.title;
    this.questions = spojnica.questions;
  }

  render() {
    const spojnicaContainer = document.getElementById("spojnicaContainer");
    const spojnicaTitle = document.createElement("h2");
    spojnicaTitle.innerText = this.title;
    spojnicaContainer.appendChild(spojnicaTitle);

    const spojnicaQuestions = document.createElement("div");
    spojnicaQuestions.classList.add("spojnicaQuestions");
    this.questions.forEach((question) => {
      const row = document.createElement("div");
      row.className = "row";
      const questionItem = document.createElement("div");
      const answerItem = document.createElement("div");

      questionItem.innerText = question.question;
      questionItem.id = `q-${question.id}`;
      answerItem.innerText = question.answer;
      answerItem.id = `a-${question.id}`;

      row.appendChild(questionItem);
      row.appendChild(answerItem);
      spojnicaQuestions.appendChild(row);
    });
    spojnicaContainer.appendChild(spojnicaQuestions);
  }
}
