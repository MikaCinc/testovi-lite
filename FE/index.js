// import { shuffleArray } from "./common.js";
import Spojnica from "./spojnica.js";

let mockSpojnica = {
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

window.onload = () => {
  const spojnica = new Spojnica(mockSpojnica);
  spojnica.start();
};
