const shuffleArray = (arr) => {
  let array = [...arr];
  for (var i = array.length - 1; i > 0; i--) {
    var j = Math.floor(Math.random() * (i + 1));
    var temp = array[i];
    array[i] = array[j];
    array[j] = temp;
  }

  return array;
};

const getApiURL = () => {
  return "https://localhost:3001/";
};

const fetchAllDataInSet = (setId, callback) => {
  Promise.all([
    fetch(getApiURL() + "Tag/PreuzmiTagove/" + setId, {
      method: "get",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
    }).then((res) => res.json()),
    fetch(getApiURL() + "Pitanje/PreuzmiPitanja/" + setId, {
      method: "get",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
    }).then((res) => res.json()),
    fetch(getApiURL() + "Spojnica/PreuzmiSpojnice/" + setId, {
      method: "get",
      headers: new Headers({
        "Content-Type": "application/json",
      }),
    }).then((res) => res.json()),
  ]).then(([tagovi, pitanja, spojnice]) => {
    console.log("fetchAllDataInSet", tagovi, pitanja, spojnice);
    let objToReturn = {
      spojnice: [],
      pitanja: [],
      tagovi: [],
    };
    if (tagovi && tagovi.length) {
      objToReturn.tagovi = tagovi;
    }
    if (pitanja && pitanja.length) {
      objToReturn.pitanja = pitanja;
    }
    if (spojnice && spojnice.length) {
      objToReturn.spojnice = spojnice;
    }

    if (callback) {
      callback(objToReturn);
    }
  });
};

export { shuffleArray, getApiURL, fetchAllDataInSet };
