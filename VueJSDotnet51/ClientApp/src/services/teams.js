import { http } from './config'
export default {
    list: () => {
        return http.get('team')
    },
    save: (book) => {
        return http.post('team', book)
    },
    delete: (id) => {
        return http.delete('team/' + id)
    }
}