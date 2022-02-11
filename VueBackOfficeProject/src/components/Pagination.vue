<template>
    <div>
        <ul class="pagination justify-content-center">
            <button @click="previousPage()" class="btn btn-primary ">Previous</button>
            <button v-for="(item,index) in arrayPages" :key="index" @click="changePage(item)" class="page-item page-link"  v-bind:class="{'bg-primary': isCurrent(item),'text-white':isCurrent(item) }">{{item}}</button>
            <button @click="nextPage()" class="btn btn-primary">Next</button>
        </ul>
    </div>
</template>

<script>

export default {
  data(){
   return {
     currentpage:1,
     arrayPages:[],
     totalPagesNumber2:1
   }
  }, 
  props:{
      totalPagesNumber: Number,
      page: Number
  }
  
  ,

  methods:{
    fetchPage(){
        this.$emit('changePage', this.currentpage)
    },
    //increase the current page by one and refresh the data
    nextPage(){
      if(this.currentpage < this.totalPagesNumber2)
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
      p+1>this.totalPagesNumber2? p:x.push(p+1)
      p+2>this.totalPagesNumber2? p:x.push(p+2)
      this.arrayPages = x
    },
  },
  created(){
      this.totalPagesNumber2 = this.totalPagesNumber
    this.closePages()
  },

  watch:{
      totalPagesNumber(){
          this.closePages()
      },
      page() {
        this.closePages()
      }
  }


  
}
</script>

