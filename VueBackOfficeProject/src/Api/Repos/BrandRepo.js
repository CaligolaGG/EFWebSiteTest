import Base from "../Base";
const resource = '/Brand';

export default {
    getAllNames(){
        return Base.get(`${resource}`+"/GetNames")
    },
    get(page) {
        return Base.get(`${resource}`+"/ProductPage/"+page);
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