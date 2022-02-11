
import Base from "../Base";
const resource = '/Product';

export default {
    //gets a PAGE of products
    get(page,orderBy="0",isAsc=true,brandName="",pagesize=10) {
        return Base.get(`${resource}`+"/ProductPage/"+page+"/"+ pagesize +"/"+orderBy+"/"+isAsc+"/"+brandName);
    },
    //creates a product without categories
    create(payload) {
        return Base.post(`${resource}`+"/"+"InsertProduct", payload);
    },
    //creates a product with categories
    createWithCats(payload){
        return Base.post(`${resource}`+"/"+"InsertProductCat", payload);
    },
    //get a product details
    getProductDetail(id) {
        return Base.get(`${resource}/ProductDetail/${id}`);
    },
    //get an entire products
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