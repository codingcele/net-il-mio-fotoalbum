function loadImages(searchKey, isAdmin) {
    axios.get('/Home', {
        params: {
            str: searchKey
        }
    })
        .then((res) => {        //se la richiesta va a buon fine
            console.log('risposta ok', res);
            console.log("isAdmin:", isAdmin);
            if (res.data.length == 0) {     //non ci sono post da mostrare => nascondo la tabella
                document.getElementById('images').classList.add('d-none');
                document.getElementById('filtered-images').classList.add('d-none');
                document.getElementById('no-images').classList.remove('d-none');
            } else {                        //ci sono post da mostrare => visualizzo la tabella
                document.getElementById('images').classList.add('d-none');
                document.getElementById('no-images').classList.add('d-none');
                document.getElementById('filtered-images').classList.remove('d-none');

                //svuoto la tabella
                document.getElementById('filtered-images').innerHTML = '';
                res.data.forEach(image => {
                    console.log('image', image);

                    // Creazione dell'elemento div
                    var rowDiv = document.createElement('div');
                    rowDiv.classList.add('row', 'mb-3');

                    // Creazione dell'elemento interno al div
                    var colDiv = document.createElement('div');
                    colDiv.classList.add('col', 'd-flex', 'align-items-center', 'fs-4', 'pb-2');

                    // Aggiunta del primo elemento col-3
                    var imgDiv = document.createElement('div');
                    imgDiv.classList.add('col-3');
                    var imageElement = document.createElement('img');
                    imageElement.src = `./img/${image.picture}`;
                    imageElement.style.maxWidth = '250px';
                    imageElement.style.maxHeight = '170px';
                    imgDiv.appendChild(imageElement);
                    colDiv.appendChild(imgDiv);

                    // Aggiunta del secondo elemento col-2
                    var titleDiv = document.createElement('div');
                    titleDiv.classList.add('col-2', 'text-center');
                    titleDiv.textContent = image.title;
                    colDiv.appendChild(titleDiv);

                    // Aggiunta del terzo elemento col-4
                    var categoriesDiv = document.createElement('div');
                    categoriesDiv.classList.add('col-4', 'text-center');
                    image.categories.forEach((category, index) => {
                        var categorySpan = document.createElement('span');
                        categorySpan.textContent = category.name;
                        if (index !== image.categories.length - 1) {
                            categorySpan.textContent += ',';
                        }
                        categoriesDiv.appendChild(categorySpan);
                    });
                    colDiv.appendChild(categoriesDiv);

                    // Aggiunta del quarto elemento col-3
                    var actionsDiv = document.createElement('div');
                    actionsDiv.classList.add('col-3', 'text-center', 'pl-4');
                    if (isAdmin) {
                        var updateLink = document.createElement('a');
                        updateLink.href = `/Home/Update?id=${image.id}`;
                        updateLink.classList.add('btn');
                        updateLink.style.backgroundColor = 'green';
                        updateLink.style.color = 'white';
                        updateLink.textContent = 'Modifica';
                        actionsDiv.appendChild(updateLink);
                    }
                    var detailsLink = document.createElement('a');
                    detailsLink.href = `/Home/Details?id=${image.id}`;
                    detailsLink.classList.add('btn', 'btn-primary');
                    detailsLink.textContent = 'Dettagli';
                    actionsDiv.appendChild(detailsLink);
                    if (isAdmin) {
                        var deleteForm = document.createElement('form');
                        deleteForm.action = `/Home/Delete/${image.id}`;
                        deleteForm.method = 'post';

                        var hiddenMethodInput = document.createElement('input');
                        hiddenMethodInput.type = 'hidden';
                        hiddenMethodInput.name = '_method';
                        hiddenMethodInput.value = 'delete';
                        deleteForm.appendChild(hiddenMethodInput);

                        var hiddenTokenInput = document.createElement('input');
                        hiddenTokenInput.type = 'hidden';
                        hiddenTokenInput.name = '_token';
                        hiddenTokenInput.value = '{{ csrf_token() }}';
                        deleteForm.appendChild(hiddenTokenInput);

                        var deleteButton = document.createElement('button');
                        deleteButton.type = 'submit';
                        deleteButton.classList.add('btn', 'btn-danger', 'btn-sm');
                        deleteButton.textContent = 'Elimina';
                        deleteForm.appendChild(deleteButton);

                        actionsDiv.appendChild(deleteForm);
                    }
                    colDiv.appendChild(actionsDiv);

                    rowDiv.appendChild(colDiv);
                    document.getElementById('filtered-images').appendChild(rowDiv);
                });
            }
        })
        .catch((res) => {       //se la richiesta non è andata a buon fine
            console.error('errore', res);
            alert('errore lista immagini');
        });
}
