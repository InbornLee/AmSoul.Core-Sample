import { http } from './config'
export default {

    list: () => {
        return http.get('book')
    },

    save: (book) => {
        return http.post('book', book)
    },
    delete: (id) => {
        return http.delete('book/'+id)
    }
}