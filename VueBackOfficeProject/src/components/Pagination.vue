<template>
    <div class="input-group my-4">
        <div class="pagination justify-content-center input-group">
            <button @click="previousPage()" v-bind:class="{'btn':true , 'btn-outline-secondary':this.currentpage ==1 ,'btn-outline-primary':this.currentpage != 1} " v-bind:disabled= "this.currentpage == 1">Previous</button>
            <button v-if="currentpage >2" class="page-item page-link " @click="changePage(1)">1</button>
            <button v-for="(item,index) in closePages()" :key="index" @click="changePage(item)" 
              class="page-item page-link"  v-bind:class="{'bg-primary': isCurrent(item), 'text-white':isCurrent(item), 'active': !isCurrent(item) }"  >
              {{item}}
            </button>
            <button v-if="currentpage < totalPagesNumber-2" @click="changePage(totalPagesNumber)" class="page-item page-link"> {{totalPagesNumber}}</button>
            <button @click="nextPage()" 
            v-bind:class="{'btn':true, 'btn-outline-secondary':this.currentpage == this.totalPagesNumber,'btn-outline-primary':this.currentpage != this.totalPagesNumber}" v-bind:disabled="this.currentpage == this.totalPagesNumber">Next</button>
        </div>
    </div>
</template>

<script>

export default {
  data(){
   return {
     currentpage:1,
   }
  }, 
  props:{
      totalPagesNumber: Number,
      page: Number
  }
  
  ,

  methods:{
    //emit the event for the fetching of a page
    fetchPage(){
        this.$emit('changePage', this.currentpage)
    },
    //increase the current page by one and refresh the data
    nextPage(){
      if(this.currentpage < this.totalPagesNumber)
        ++ this.currentpage;
       this.fetchPage();
    },
    //decrease the current page by one and refresh the data
    previousPage(){
      if(this.currentpage > 1)
        -- this.currentpage;
       this.fetchPage();    
    },
    //change the current page to a specific one and refresh the data
    changePage(pageNum){
      this.currentpage = pageNum;
      this.fetchPage();
    },
    //return true or false if the parameter passed is equal the current page
    isCurrent(x){
      return this.currentpage == x 
    },
    //returns an array with the closest pages to the current one
    closePages(){
      let x=[]
      let p= this.currentpage
      p-2<2? p:x.push(p-2)
      p-1<1? p:x.push(p-1)
      x.push(p);
      p+1>this.totalPagesNumber? p:x.push(p+1)
      p+2>this.totalPagesNumber? p:x.push(p+2)
      return x
    },
  },
  created(){
    this.closePages()
  },
  
  watch:{
    totalPagesNumber(){
      this.currentpage=1
    }
  }


  
}
</script>

