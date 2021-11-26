class Home {
    constructor() {
        this.state = {

        }
    }


    render() {
        const container = document.createElement('div');
        container.classList.add('homeContainer');

        const searchContainer = document.createElement('div');
        searchContainer.classList.add('searchContainer');

        const searchInput = document.createElement('input');
        searchInput.classList.add('searchInput');
        searchInput.setAttribute('type', 'text');
        searchInput.setAttribute('placeholder', 'Pretraži pitanja i spojnice');

        const submitButton = document.createElement('button');
        submitButton.classList.add('button submitButton');

        searchContainer.appendChild(searchInput);
        searchContainer.appendChild(submitButton);
        container.appendChild(searchContainer);
    }
}

export default Home;