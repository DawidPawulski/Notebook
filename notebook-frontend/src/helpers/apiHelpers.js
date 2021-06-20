const reactAppApi = 'https://localhost:5001/api/';

export const get = (path) => {
    return fetch(reactAppApi + path)
        .then((response) => response.json())
        .catch((error) => {
            alert('Failed to get')
        });
}

export const create = (path, body) =>{
    return fetch(reactAppApi + path, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: body
        })
        .then((response) => response.json())
        .catch((error) => {
            alert('Failed to create')
        });
}

export const update = (path, body={}) =>{
    return fetch(reactAppApi + path, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: body
        })
        .then((response) => response.json())
        .catch((error) => {
            alert('Failed to update')
        });
}

export const remove = (path) => {
    return fetch(reactAppApi + path, {
            method: 'DELETE'
        })
        .then((response) => JSON.parse(JSON.stringify(response)))
        .catch((error) => {
            console.log(error);
            alert('Failed to delete')
        });
}