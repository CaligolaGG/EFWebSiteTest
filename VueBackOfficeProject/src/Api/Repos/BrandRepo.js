import Base from "../Base";
const resource = '/Brand';

export default {
    //get an entire brand object by its id
    getById(id){
        return Base.get(`${resource}/GetBrand/${id}`)
    },
    //get a page of brands
    getAll(pageNum,searchByName,pagesize) {
        return Base.get(`${resource}/BrandPage/${pageNum}/${pagesize}/${searchByName}`);
    },
    //get brand details with its products
    getBrand(id) {
        return Base.get(`${resource}/BrandDetail/${id}`);
    },
    //get a list of all brands names and ids
    get(){
        return Base.get(`${resource}`)
    },
    delete(id) {
        return Base.delete(`${resource}/${id}`)
    },
    
    create(payload) {
        return Base.post(`${resource}`, payload);
    },
    update(payload, id) {
        return Base.put(`${resource}/${id}`, payload);
    },


};