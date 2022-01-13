// import { shuffleArray } from "./common.js";
import Spojnica from "./spojnica.js";
import Home from "./home.js";
import { getApiURL, fetchAllDataInSet } from "./common.js";

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
  setovi: [],
  spojnice: [],
  pitanja: [],
  tagovi: [],
};

window.onload = () => {
  fetch(getApiURL() + "Set/PreuzmiSetove")
    .then((response) => response.json())
    .then((sets) => {
      if (sets && sets.length && sets[0].id) {
        console.log("Setovi: ", sets);
        state.setovi = sets;
        /* Promise.all([
          fetch(getApiURL() + "Tag/PreuzmiTagove/" + sets[0].id, {
            method: "get",
            headers: new Headers({
              "Content-Type": "application/json",
            }),
          }).then((res) => res.json()),
          fetch(getApiURL() + "Pitanje/PreuzmiPitanja/" + sets[0].id, {
            method: "get",
            headers: new Headers({
              "Content-Type": "application/json",
            }),
          }).then((res) => res.json()),
          fetch(getApiURL() + "Spojnica/PreuzmiSpojnice/" + sets[0].id, {
            method: "get",
            headers: new Headers({
              "Content-Type": "application/json",
            }),
          }).then((res) => res.json()),
        ]).then(([tagovi, pitanja, spojnice]) => {
          console.log(tagovi, pitanja, spojnice);
          if (tagovi && tagovi.length) {
            state.tagovi = tagovi;
          }
          if (pitanja && pitanja.length) {
            state.pitanja = pitanja;
          }
          if (spojnice && spojnice.length) {
            state.spojnice = spojnice;
          }

          const home = new Home(state);
          home.render();
        }); */

        fetchAllDataInSet(sets[0].id, ({ tagovi, pitanja, spojnice }) => {
          state.tagovi = tagovi;
          state.pitanja = pitanja;
          state.spojnice = spojnice;

          const home = new Home(state);
          home.render();
        });
      }
    })
    .catch();

  /* const spojnica = new Spojnica(mockSpojnica);
  spojnica.start(); */
};

export default state;
