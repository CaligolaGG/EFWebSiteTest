import Base from "../Base";
const resource = '/Category';

export default {

    get() {
        return Base.get(`${resource}`);
    },


    getPost(id) {
        return Base.get(`${resource}/${id}`);
    },
    create(payload) {
        return Base.post(`${resource}`, payload);
    },
    update(payload, id) {
        return Base.put(`${resource}/${id}`, payload);
    },
    delete(id) {
        return Base.delete(`${resource}/${id}`)
    },

};