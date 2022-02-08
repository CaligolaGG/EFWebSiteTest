
import Base from "../Base";
const resource = '/Product';

export default {
    get(page,orderBy="0",isAsc=true,brandName="",pagesize=10) {
        return Base.get(`${resource}`+"/ProductPage/"+page+"/"+ pagesize +"/"+orderBy+"/"+isAsc+"/"+brandName);
    },
    create(payload) {
        return Base.post(`${resource}`+"/"+"InsertProduct", payload);
    },
    createWithCats(payload){
        return Base.post(`${resource}`+"/"+"InsertProductCat", payload);
    },
    getProductDetail(id) {
        return Base.get(`${resource}/ProductDetail/${id}`);
    },
    getProduct(id) {
        return Base.get(`${resource}/${id}`);
    },
    update(payload) {
        return Base.put(`${resource}/UpdateProductCat`, payload);
    },
    delete(id) {
        return Base.delete(`${resource}/DeleteProductL/${id}`)
    },

};