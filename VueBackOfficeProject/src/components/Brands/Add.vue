<template>
    <div class="container">
        <form  v-if="!this.loading" id="insert" v-on:submit.prevent="submitForm()">
            <h2> Add New Brand </h2>
            <div class="alert alert-danger" role="alert" v-bind:class="{'d-none':!alertActive}" fade>
                Two product have the same name. Brand has not been created
            </div>
            <div class="form-group mb-2">
                <label for="pname">Name</label>
                <input required type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="brand.BrandName">
            </div>
            <div class="form-group mb-2">
                <label for="desc">Description</label>
                <input type="textarea" class="form-control" name="desc"  maxlength="50" v-model="brand.Description">
            </div>

            <div class="form-group mb-2">
                <div v-bind:class="{'invalid-feedback':validMail,'text-danger':true}">
                   Please choose a valid mail.
                </div>
                <label for="desc">Email</label>
                <input required  type="text" class="form-control" name="desc"  maxlength="50" v-model="account.Email">
            </div>
            <div class="form-group mb-2">
                <label for="desc">Password</label>
                <input  required minlength="5" type="password" class="form-control" name="desc"  maxlength="50" v-model="account.Password">
            </div>


        
            <div v-for="n in this.numProducts" :key="n" class="my-3">
                <form  class="bg-light my-2 border " >
                    <div class="mx-2">
                        <b> Insert new product</b>
                        <div class="form-group mb-2">
                            <label for="pname">Name</label>
                            <input required type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="bundles[n-1].Product.Name">
                        </div>
                        <div class="form-group mb-2">
                            <label for="desc">Description</label>
                            <input type="textarea" class="form-control"  maxlength="50" name="desc" v-model="bundles[n-1].Product.Description">
                        </div>
                        <div class="form-group mb-2">
                            <label for="sdesc">ShortDescription</label>
                            <input type="textarea" class="form-control"   maxlength="20" name="sdesc" v-model="bundles[n-1].Product.ShortDescription">
                        </div>
                        <div class="form-group mb-2">
                            <label for="price">Price</label>
                            <input min="1" type="number" class="form-control" name="price" v-model.number="bundles[n-1].Product.Price">
                        </div>
                        <div class="form-group mb-2 row my-4">
                            <b> Categories </b>
                            <div class="col">
                                <div class="mx-2 row">
                                    <li v-for="cat in categories" :key="cat.Id" class="form-check col-4">
                                        <input type="checkbox"  v-model="bundles[n-1].Categories" v-bind:value="cat.id"  class="form-check-input">
                                        <label> {{cat.name}} </label> 
                                    </li>
                                </div>
                            </div>

                    </div>
                </div>
             </form>

            
        </div>
            <button type="submit" class="btn btn-outline-primary my-2">Submit</button>

        </form>
        <button @click="addProduct()" class="btn btn-outline-primary mb-4">Add Product</button>





</div>
</template>



<script>

import Repository from "../../Api/RepoFactory";
const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");


export default {
    data() {
        return {
            id:0,       //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            info:{},      //object that contains the info of the brand fetched
            numProducts: 0, //number of Product forms
            
            //object to contains the brand account infos from the form
            account:{
                Email:"",
                Password:"",
                AccountType:2
            },
            //object to contains the brand info from the form
            brand:{
                BrandName:"",
                Description:"",
                isDeleted:0
            },
            categories: {}, //object to contains a list of cateories 

            bundles:[], //list of Bundle objects
            error:"", 

            validMail:true,
            alertActive: false,

        }    
    },
    methods:{
        //get the brand data by calling the specific api
        async getData(){ 
            let cats = await CategoriesRepository.get();
            this.categories = cats.data;
            this.loading=false;
        },
        //method to submit the form
        //checks if there are duplicated names in products.
        //redirect to the created brand if this has been created correctly
        async submitForm(){
            var names = []
            if(this.bundles.length!=0 )
                for (var p of this.bundles)
                    names.push(p.Product.Name)

            
            if (this.checkProducts(names))
                this.alertActive=true
            else 
            if(this.validateEmail(this.account.Email))
            {
                this.alertActive=false
                var id = await BrandRepository.create( {Brand : this.brand , ProductsCategs: this.bundles, Account:this.account})
                                                .catch( (response)=> this.error = response);

                if(id.status!=200)
                {
                    console.log(id)
                    alert("brand name or email already taken. Brand NOT inserted")
                }
                else
                    this.$router.push({path:'/brands/'+id.data})
            }
            else this.validMail = false
        },
        //adds a new Product form, insert a new Bundle object into bundles.
        addProduct(){
            this.numProducts++;
            this.bundles.push(new bundle())
        },
        //checks for any duplicated elements in an array. returns true if any is found
        checkProducts(strArray){
            const alreadySeen = [];
            var isDuplicates = false
            for (var str of strArray )
            {
                if (alreadySeen[str])
                {
                    isDuplicates=true
                    break
                }
                alreadySeen[str] = true
            }
            //strArray.forEach(str => alreadySeen[str] ? isDuplicates=true : alreadySeen[str] = true)
            return isDuplicates
        },
        validateEmail (email)
        {
            return String(email)
            .toLowerCase()
            .match(
            /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            );
        },
        showToast(){

        }



    },
    async created(){
        this.id = this.$route.params.id;
        await this.getData();
    }
    
}

//class that holds a product with its categories
var bundle = function(){
            this.Product=//object to hold the data inserted in the form that has to be passed to the api
            {
                Name:"",
                Description:"",
                ShortDescription:"",
                Price:1
            },
            this.Categories=[] //array to hold the list of classes selected for the product to insert 
}


</script>
