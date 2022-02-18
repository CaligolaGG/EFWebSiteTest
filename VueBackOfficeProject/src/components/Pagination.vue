<template>
    <div class="input-group my-4">
        <div class="pagination justify-content-center input-group">
            <button @click="previousPage()" 
              v-bind:class="{'btn':true , 'btn-outline-secondary':this.currentpage ==1 ,'btn-outline-primary':this.currentpage != 1} " 
              v-bind:disabled="this.currentpage == 1">
              Previous
            </button>
            <button v-if="currentpage >2" class="page-item page-link " @click="changePage(1)">1</button>
            <button v-for="(item,index) in closePages()" :key="index" @click="changePage(item)" 
              class="page-item page-link"   v-bind:disabled="currentpage == item"
              v-bind:class="{'bg-primary': isCurrent(item), 'text-white':isCurrent(item), 'active': !isCurrent(item) }"  >
              {{item}}
            </button>
            <button v-if="currentpage < totalPagesNumber-2" @click="changePage(totalPagesNumber)" class="page-item page-link"> {{totalPagesNumber}}</button>
            <input type="text" class="page-item page-link col-1 " placeholder="select page" 
            v-debounce:700ms="selectPage"  v-model="customPage"  v-if="totalPagesNumber != 1" >
            <button @click="nextPage()" 
              v-bind:class="{'btn':true, 'btn-outline-secondary':this.currentpage == this.totalPagesNumber,'btn-outline-primary':this.currentpage != this.totalPagesNumber}"
              v-bind:disabled="this.currentpage == this.totalPagesNumber">
              Next
            </button>
        </div>
    </div>
</template>

<script>

export default {
  data(){
   return {
     currentpage:1,
     customPage:null,
   }
  }, 
  props:{
      totalPagesNumber: Number,
      page: Number,
      resetTo: Number,
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

    selectPage(){
      if(this.customPage < this.totalPagesNumber && this.customPage && this.customPage > 0)
        this.currentpage = parseInt(this.customPage)
      else
        this.currentpage = 1
      this.fetchPage()

    }
  },
  created(){
    this.closePages()
  },
  
  watch:{
    resetTo(){
      this.currentpage=this.resetTo
    }
  }
  
}
</script>

<style scoped>
  .page-link::placeholder { 
            color: rgba(3, 90, 221, 0.377);
            opacity: 1;
}
</style>