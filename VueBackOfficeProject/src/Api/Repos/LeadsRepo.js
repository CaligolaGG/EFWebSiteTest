import Base from "../Base";
const resource = '/Request';

export default {
    getPage(pageNum=1,brandId=0,productName=null,isAsc=false,pageSize=10) {
        if(productName === null)
            return Base.get(`${resource}/LeadsPage/${pageNum}/${pageSize}/${brandId}/${isAsc}`);
        else
            return Base.get(`${resource}/LeadsPage/${pageNum}/${pageSize}/${brandId}/${isAsc}/${productName}`);
    },
    getById (id) {
        return Base.get(`${resource}/RequestDetail/${id}`);
    },

    
    get() {
        return Base.get(`${resource}`);
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