import axios from "axios"
 
const api = {
    getAllProcess: () => {
        return axios.get("https://localhost:7217/dev/workflow/all", {
            headers: {
            'Content-Type': 'application/json'
            }
        });
    },
    getProcess: (processId) => {
        return axios.get(`https://localhost:7217/dev/workflow/${processId}`, {
            headers: {
            'Content-Type': 'application/json'
            }
        });
    },
    deleteProcess: (id) => {
        return axios.delete(`https://localhost:7217/dev/workflow/${id}`, {
            headers: {
              'Content-Type': 'application/json'
            }
        })
    },
    quickRun: (headers, payload) => {
        return axios.post("https://localhost:7217/dev/workflow/quickrun", encrypt(JSON.stringify(payload), import.meta.env.VITE_SECRET), headers)
    }
}

export default api;