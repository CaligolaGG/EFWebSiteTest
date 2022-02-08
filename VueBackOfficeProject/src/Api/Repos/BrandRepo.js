import Base from "../Base";
const resource = '/Brand';

export default {
    getAll(){
        return Base.get(`${resource}`)
    },
    get(page) {
        return Base.get(`${resource}`+"/BrandPage/"+page);
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