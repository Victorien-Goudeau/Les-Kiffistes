export function useApi() {

    const callApi = async (method: string, route: string, body?: string | null) => {
        let token = sessionStorage.getItem("token") || null;
        console.log("Token from localStorage:", token);
        if (token === null) {
            console.log("Token not found in localStorage, checking sessionStorage...");
            token = localStorage.getItem("token") || null;
        }
        const options: RequestInit = {
            method: method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,
            },
            body: body != "" ? body : null,
        };

        // return await fetch(`https://spa-chatbot-poc-e8fvd7eccpfxhga7.francecentral-01.azurewebsites.net/api/${route}`, options)
        return await fetch(`https://localhost:5001/api/${route}`, options)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok " + response.statusText);
                }
                if (response.status !== 200) {
                    return response;
                }
                return response;
            })
            .then(data => {
                return data;
            })
            .catch(error => {
                throw new Error("Error fetching data: " + error.message);
            });
    };

    return { callApi };
}