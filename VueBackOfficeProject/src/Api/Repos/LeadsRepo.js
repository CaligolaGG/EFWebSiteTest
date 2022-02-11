import Base from "../Base";
const resource = '/Request';

export default {
    //get a page of leads
    getPage(pageNum=1,brandId=0,productName=null,isAsc=false,pageSize=10) {
        if(productName === null)
            return Base.get(`${resource}/LeadsPage/${pageNum}/${pageSize}/${brandId}/${isAsc}`);
        else
            return Base.get(`${resource}/LeadsPage/${pageNum}/${pageSize}/${brandId}/${isAsc}/${productName}`);
    },
    //get a specific lead by its id
    getById (id) {
        return Base.get(`${resource}/RequestDetail/${id}`);
    },


};