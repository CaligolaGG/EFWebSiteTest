<template>
    <div class="container">
        <form  v-if="!this.loading" id="insert" v-on:submit.prevent="submitForm()">
            <h2> Add New Brand </h2>

            <div class="alert alert-danger my-1 fade-out" role="alert" v-bind:class="{'d-none':!alertActive}" >
                {{alertText}}
                <button type="button" class="btn-close float-end" @click="alertActive = false"  aria-label="Close"></button>
            </div>
            

            <div class="form-group mt-3 mb-2">
                <input placeholder="Name" required type="text" name="pname" id="" class="form-control bg-light"  maxlength="50" v-model="brand.BrandName">
            </div>
            <div class="form-group mb-2">
                <textarea placeholder="Description" class="form-control bg-light" name="desc"  maxlength="50" v-model="brand.Description" style="resize:none;" rows=5></textarea>
            </div>

            <div class="form-group mb-2">
                <div v-bind:class="{'invalid-feedback':validMail,'text-danger':true}">
                   Please choose a valid mail.
                </div>
                <input placeholder="Email" required  type="text" class="form-control bg-light" name="desc"  maxlength="50" v-model="account.Email">
            </div>
            <div class="form-group mb-2">
                <input placeholder="Password"  required minlength="5" type="password" class="form-control bg-light" name="desc"  maxlength="50" v-model="account.Password">
            </div>


            <h2 class="my-3 mt-5" v-if="this.numProducts > 0">Products</h2>
        
            <div v-for="n in this.numProducts" :key="n" class="my-3">
                <form  class="bg-light my-2 border " >
                    <div class="mx-2">
                        <div class="row">
                            <h5 class="col pt-3"><b> Insert Product {{n}} </b></h5>
                            <div class="col-1 ">
                                <button type="button" class=" btn btn-danger rounded-circle my-2 float-end" @click="removeProduct(n)">X</button>
                            </div>
                        </div>
                        <div class="form-group mb-2">
                            <input placeholder="Name" required type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="bundles[n-1].Name">
                        </div>
                        <div class="form-group mb-2">
                            <textarea placeholder="Description" type="textarea" class="form-control"  maxlength="50" name="desc" v-model="bundles[n-1].Description" style="resize:none;" rows=5></textarea>
                        </div>
                        <div class="row">
                            <div class="form-group mb-2 col-8">
                                <input placeholder="ShortDescription" type="textarea" class="form-control"   maxlength="20" name="sdesc" v-model="bundles[n-1].ShortDescription">
                            </div>
                            <div class="col">
                                <div class="input-group ">
                                    <label class="input-group-text ">
                                        <span for="price">Price</span></label>
                                    <div class="col-10">
                                        <input placeholder="Price" min="1" step=".0001"  type="number" class="form-control" name="price" v-model.number="bundles[n-1].Price">
                                    </div>
                                </div>
                            </div>
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
            <div class="offset-4 col-4 mb-4">
                <button type="button"  @click="addProduct()" class="btn btn-outline-primary m-2 ">+ Add Product</button>
                <button  @keyup.enter="submitForm()" type="submit" class="btn btn-outline-primary m-2">Submit</button>
            </div>
        </form>

    </div>
</template>

<script>
import Repository from "../../Api/RepoFactory";
const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");
import Utilities from "../../Utilities/utilityFunctions.js";



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
            categories: {},         //object to contains a list of cateories 

            bundles:[],             //list of Bundle objects
            error:false,            //indicates if an error was raised

            validMail:true,         //used to check if the mail is valid or not
            alertActive: false,     //indicates if the alert div should be shown or not
            alertText:"",

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
        //checks if the field are valid or if there are duplicated names in products.
        //if not activates the alert div.
        //redirect to the created brand if this has been created correctly
        async submitForm(){
            var names = []
            this.error = false
            this.alertActive =false

            if(this.bundles.length!=0 )
                for (var p of this.bundles)
                    names.push(p.Name)


            //form checks
            if( Utilities.isStringInvalid (this.brand.BrandName)  )
            {
                this.alertText = "Brand name invalid"
                window.scrollTo(0, 0);
                this.error = true
                this.alertActive = true
            }

            for (var name of names)
                if( Utilities.isStringInvalid (name))
                {
                    this.alertText ="One or more Products name invalid"
                    window.scrollTo(0, 0);
                    this.error = true
                    this.alertActive = true
                    break;
                }

            if (this.checkProducts(names))
            {
                this.alertText = "Two or more product have the same name"
                window.scrollTo(0, 0);
                this.error = true
                this.alertActive=true
            }
            
            if(!this.validateEmail(this.account.Email))
            {
                this.validMail = false
                this.error = true
                window.scrollTo(0, 0);
            }
            else
                this.validMail = true


            //if no error in the form try to insert/update  
            if(!this.error) 
            {
                this.alertActive=false
                this.validMail = true

                var id = await BrandRepository.create( {Brand : this.brand , ProductsCategs: this.bundles, Account:this.account})
                                                .catch( (response)=> this.error = response);

                if(id.status!=200)
                {
                    if(this.error.response.status == 400)
                    {
                        this.alertText = ""
                        for (var key in this.error.response.data)
                            this.alertText +=  key + " already taken. "
                        window.scrollTo(0, 0);
                        this.alertActive=true
                    }
                    else
                    {
                        this.alertText = "An unexpected error occurred Retry Later"
                        window.scrollTo(0, 0);
                        this.alertActive=true
                    }
                }
                else
                    this.$router.push({path:'/brands/'+id.data})
            }

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
        removeProduct(n){
            this.bundles.splice(n-1,1);
            this.numProducts--;
        },

    },
    async created(){
        this.id = this.$route.params.id;
        await this.getData();
    },
    watch:{
        alertActive(){
            if(this.alertActive == true)
               setTimeout(()=> this.alertActive = false, 3800)
        }
    }
    
}

//class that holds a product with its categories
var bundle = function(){
            //this.Product=//object to hold the data inserted in the form that has to be passed to the api
            //{
                this.Name=""
                this.Description=""
                this.ShortDescription=""
                this.Price=1
            //},
            this.Categories=[] //array to hold the list of classes selected for the product to insert 
}


</script>

<style scoped>
  .form-control::placeholder { 
            color: rgba(15, 15, 15, 0.377);
            opacity: 1;
}
</style>