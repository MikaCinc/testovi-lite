import { shuffleArray } from "./common.js";

class Spojnica {
  constructor(spojnica) {
    this.id = spojnica.id;
    this.title = spojnica.title;
    this.questions = shuffleArray(spojnica.questions);
    this.answers = shuffleArray(spojnica.questions).map((question) => ({
      id: question.id,
      answer: question.answer,
    }));
    this.tags = spojnica.tags;

    this.completed = []; // id array of completed questions
    this.currentIndex = 0; // index of current question
    this.currentQuestion = this.questions[0].id; // id of current question
    this.selectedAnswer = null; // id of selected answer

    this.showNewQuestion = false;
  }

  checkAnswer() {
    if (this.currentQuestion === this.selectedAnswer) {
      this.completed.push(this.currentQuestion);
    }

    this.nextQuestion();
  }

  isFinished() {
    return this.currentIndex >= this.questions.length;
  }

  nextQuestion() {
    this.currentIndex++;
    if (!this.isFinished()) {
      this.currentQuestion = this.questions[this.currentIndex].id;
      this.selectedAnswer = null;
    }
    this.render();
  }

  start() {
    this.completed = [];
    this.currentIndex = 0;
    this.currentQuestion = this.questions[0].id;
    this.selectedAnswer = null;
    this.render();
  }

  addNewQuestion(question, answer) {
    if (!question || !answer) return;

    // Dodaje globalno novo pitanje u tabelu Pitanja
    // I trenutnoj spojnici dodaje isto to pitanje

    /* mockSpojnica = {
      ...mockSpojnica,
      questions: [
        ...mockSpojnica.questions,
        {
          id: `q-${mockSpojnica.questions.length + 1}`,
          question,
          answer,
        },
      ],
    };

    this.questions = shuffleArray(mockSpojnica.questions);
    this.answers = shuffleArray(mockSpojnica.questions).map((question) => ({
      id: question.id,
      answer: question.answer,
    })); */

    this.start();
  }

  showAddNewQuestion() {
    document.getElementsByClassName("addNewButton")[0].style.display = "none";
    const spojnicaContainer = document.getElementById("spojnicaContainer");

    const newContainer = document.createElement("div");
    newContainer.id = "newQuestionContainer";

    const questionInput = document.createElement("input");
    questionInput.id = "newQuestionInput";
    questionInput.placeholder = "Unesite novo pitanje";

    const answerInput = document.createElement("input");
    answerInput.id = "newAnswerInput";
    answerInput.placeholder = "Unesite odgovor";

    const addNewQuestionButton = document.createElement("div");
    addNewQuestionButton.className = "button addNewQuestionButton";
    addNewQuestionButton.innerText = "Dodaj";
    addNewQuestionButton.onclick = () => {
      let question = document.getElementById("newQuestionInput").value;
      let answer = document.getElementById("newAnswerInput").value;
      document.getElementById("newQuestionContainer").remove();
      document.getElementsByClassName("addNewButton")[0].style.display =
        "block";

      this.addNewQuestion(question, answer);
    };

    newContainer.appendChild(questionInput);
    newContainer.appendChild(answerInput);
    newContainer.appendChild(addNewQuestionButton);
    spojnicaContainer.appendChild(newContainer);
  }

  renderTags() {
    const spojnicaContainer = document.getElementById("spojnicaContainer");
    const tagsContainer = document.createElement("div");
    tagsContainer.id = "tagsContainer";

    this.tags.forEach((tag) => {
      const tagEl = document.createElement("p");
      tagEl.className = "tag";
      tagEl.innerText = tag;
      tagsContainer.appendChild(tagEl);
    });
    spojnicaContainer.appendChild(tagsContainer);
  }

  render() {
    const spojnicaContainer = document.getElementById("spojnicaContainer");
    spojnicaContainer.innerHTML = ""; // reset previous content

    const spojnicaTitle = document.createElement("h2");
    spojnicaTitle.innerText = this.title;
    spojnicaContainer.appendChild(spojnicaTitle);

    this.renderTags();

    const spojnicaQuestions = document.createElement("div");
    spojnicaQuestions.classList.add("spojnicaQuestions");
    this.questions.forEach((question, index) => {
      const isCurrent = question.id === this.currentQuestion;
      const isCorrect = this.completed.includes(question.id);
      const isIncorrect = index < this.currentIndex && !isCorrect;

      const row = document.createElement("div");
      row.className = "spojnicaQuestionsRow";
      const questionItem = document.createElement("div");
      const answerItem = document.createElement("div");

      questionItem.innerText = question.question;
      questionItem.id = `q-${question.id}`;
      questionItem.className = `spojnicaQuestionButton ${
        isCurrent && !this.isFinished() && "currentQuestion"
      } ${isCorrect && "correctQuestion"} ${
        isIncorrect && "incorrectQuestion"
      }`;

      const isCorrectAnswer = this.completed.includes(this.answers[index].id);
      answerItem.innerText = this.answers[index].answer;
      answerItem.id = `a-${question.id}`;
      answerItem.className = `spojnicaAnswerButton ${
        isCorrectAnswer && "correctAnswer"
      }`;
      answerItem.onclick = () => {
        if (this.isFinished() || answerItem.classList.contains("correctAnswer"))
          return;
        this.selectedAnswer = this.answers[index].id;
        this.checkAnswer();
      };

      row.appendChild(questionItem);
      row.appendChild(answerItem);
      spojnicaQuestions.appendChild(row);
    });
    spojnicaContainer.appendChild(spojnicaQuestions);

    const addNewButton = document.createElement("div");
    addNewButton.className = "button addNewButton";
    addNewButton.innerText = "Dodaj novo pitanje";
    addNewButton.onclick = () => {
      this.showAddNewQuestion();
    };
    spojnicaContainer.appendChild(addNewButton);
  }
}

export default Spojnica;
