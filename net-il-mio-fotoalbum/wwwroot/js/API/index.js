
function loadImages(searchKey) {
    axios.get('/Home', {
        params: {
            str: searchKey
        }
    })
        .then((res) => {        //se la richiesta va a buon fine
            console.log('risposta ok', res);
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
                    document.getElementById('filtered-images').innerHTML +=
                        `
                            <div class="row mb-3">
                                <div class="col d-flex align-items-center fs-4 pb-2">
                                    <div class="col-3">
                                        <img src="./img/${image.picture}" style="max-width: 250px; max-height: 170px;" />
                                    </div> 
                                    <div class="col-2 text-center">
                                        ${image.title}
                                    </div>
                                    <div class="col-4 text-center">
                                        ${image.categories.map(category => {
                                            if (image.categories.indexOf(category) !== image.categories.length - 1) {
                                                return `<span> ${category.name},</span>`;
                                            } else if (image.categories.length !== 1) {
                                                return `<span> ${category.name}.</span>`;
                                            } else {
                                                return `<span> ${category.name}</span>`;
                                            }
                                        }).join('')}
                                    </div>
                                    <div class="col-3 text-center pl-4">
                                        <a href="/Home/Update?id=${image.id}" class="btn" style="background-color:green; color:white;">Modifica</a>
                                        <a href="/Home/Details?id=${image.id}" class="btn btn-primary">Dettagli</a>
                
                                        <form action="/Home/Delete/${image.id}" method="post">
                                            <input type="hidden" name="_method" value="delete">
                                            <input type="hidden" name="_token" value="{{ csrf_token() }}">
                                            <button type="submit" class="btn btn-danger btn-sm">
                                                Elimina
                                            </button>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        `
                })
            }
        })
        .catch((res) => {       //se la richiesta non è andata a buon fine
            console.error('errore', res);
            alert('errore lista immagini');
        });

}