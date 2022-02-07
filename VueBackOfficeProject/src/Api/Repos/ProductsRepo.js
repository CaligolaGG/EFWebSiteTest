
import Base from "../Base";
const resource = '/Product';

export default {
    get(page,orderBy="0",isAsc=true,brandName="",pagesize=10,) {
        return Base.get(`${resource}`+"/ProductPage/"+page+"/"+ pagesize +"/"+orderBy+"/"+isAsc+"/"+brandName);
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