<template>
    <div v-if="!this.loading" class="container">
        <h3 class="mb-4">
            Information request from <b> {{info.userFullName}}</b>
            for the Product <b>{{info.productName}}</b> of Brand <b>{{info.brandName}}</b> 
        </h3>
        
        <p> <b class="mt-4"> Data of the applicant </b></p>
        <span>FullName: {{info.userFullName}} </span> <br>
        <span>UserInfo: {{info.infoUser}}</span> <br>
        <span>UserMail: {{info.email}}</span>

        <p class="mt-4"><b>Request sent by the user </b></p>
        <p>{{info.requestText}}</p>

            <div class="border border-success rounded my-3" v-for="reply in info.replies" :key="reply.Id">
                <div class="  bg-light p-4 rounded ">
                {{reply.date}} - {{reply.accountName}} 
                </div> 
                <div class=" p-4 border-top border-success  ">
                    {{reply.replyText}}
                </div>

        </div>
                    <button class="btn btn-primary  m-2">Previous</button>
            <button class="btn btn-primary">Next</button>
    </div>
</template>

<script>
import Repository from "../../Api/RepoFactory";
const LeadsRepository = Repository.get("leads");


export default {
    data() {
        return {
            id:0,  //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            
            info:{},      //object that contains the info of the product fetched

        }    
    },
    methods:{
        //get the product detail data by calling the specific api
        async getData(){ 
            let temp= await LeadsRepository.getById(this.id);
            this.info = temp.data;
            this.loading = false;
        },

    },
    async created(){
        this.id = this.$route.params.id;
        await this.getData();
    }
    
    
}
</script>