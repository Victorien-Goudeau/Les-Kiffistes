// customs/useApi.ts
import { useCallback } from "react";

export function useApi() {
    const callApi = useCallback(
        async (method: string, route: string, body?: string | null) => {
            let token = sessionStorage.getItem("token") || null;
            if (token === null) {
                token = localStorage.getItem("token") || null;
            }

            const options: RequestInit = {
                method,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: body ?? null,
            };

            const url = `https://localhost:5001/api/${route}`;

            const response = await fetch(url, options);
            if (!response.ok) {
                throw new Error(`API error: ${response.status} ${response.statusText}`);
            }
            return response;
        },
        [] // <–– aucune dépendance, stable au fil des rendus
    );

    return { callApi };
}
