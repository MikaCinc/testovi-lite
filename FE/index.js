// import { shuffleArray } from "./common.js";
import Spojnica from "./spojnica.js";
import Home from "./home.js";

let mockSpojnica = {
  id: "s-1",
  title: "Glavni gradovi",
  tags: ["Geografija", "Gradovi", "Megalopolisi"],
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
    {
      id: "4",
      question: "Rusija",
      answer: "Moskva",
    },
    {
      id: "5",
      question: "Nemačka",
      answer: "Berlin",
    },
  ],
};

let state = {
  spojnice: [{ ...mockSpojnica }],
  pitanja: [],
  tagovi: [],
};

window.onload = () => {
  const spojnica = new Spojnica(mockSpojnica);
  spojnica.start();
};
