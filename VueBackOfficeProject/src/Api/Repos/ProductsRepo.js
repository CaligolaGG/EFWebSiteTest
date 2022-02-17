import Base from "../Base";
const resource = '/Product';

export default {
    //gets a PAGE of products
    get(page,orderBy=3,isAsc=true,brandId=0,pagesize=10) {
        
        this.payload = {
            "isAsc": isAsc,
            "orderBy": orderBy,
            "brandId" : brandId,
            "pagesize" : pagesize
        }
        return Base.post(`${resource}/ProductPage/`+page,this.payload);
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
    //update a product
    update(payload) {
        return Base.put(`${resource}/UpdateProductCat`, payload);
    },
    //logical delete of a product
    delete(id) {
        return Base.delete(`${resource}/${id}`)
    },

};