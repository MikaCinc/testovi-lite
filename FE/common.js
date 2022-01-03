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

export { shuffleArray, getApiURL };
