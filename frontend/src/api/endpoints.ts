export const BASE_URL = "http://localhost:5005/api";


const endpoints = {
    auth: {
        login: `${BASE_URL}/Auth/login`,
        logout: `${BASE_URL}/Auth/logout`
    },
    documents: {
        process: `${BASE_URL}/Documents/process`,
        my: `${BASE_URL}/Documents/proccessed/my`,
        deleteHistoryItem: (taskIds: number[]) => `${BASE_URL}/Documents/task/${taskIds}`,
    }
}

export default endpoints;