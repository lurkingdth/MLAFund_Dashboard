new Vue({
    el: '#app',
    vuetify: new Vuetify(),
    data: () => ({
        dialog: false,
        dialogDelete: false,
        headers: [
          {
              text: 'Road category',
              align: 'start',
              sortable: false,
              value: 'category_road',
          },
          { text: 'category road detail', value: 'category_road_detail'},
          { text: 'Actions', value: 'actions', sortable: false },
         
        ],
        roadcategories: [],
        editedIndex: -1,
        editedItem: {
            category_road_id: 0,
            category_road: "",
            category_road_detail: ""
        },
        defaultItem: {
            category_road_id: 0,
            category_road: "",
            category_road_detail: ""
        }
    }),

    computed: {
        formTitle () {
            return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
        },
    },

    watch: {
        dialog (val) {
            val || this.close()
        },
        dialogDelete (val) {
            val || this.closeDelete()
        },
    },

    created () {
        this.initialize()
    },

    methods: {
        initialize () {
            axios.get("/api/roadcategories").then(data => this.roadcategories=data.data)
        },

        editItem (item) {
            this.editedIndex = this.roadcategories.indexOf(item)
            this.editedItem = Object.assign({}, item)
            this.dialog = true
        },

        deleteItem (item) {
            this.editedIndex = this.roadcategories.indexOf(item)
            this.editedItem = Object.assign({}, item)
            this.dialogDelete = true
        },

        deleteItemConfirm () {

            //api--
            this.roadcategories.splice(this.editedIndex, 1)
            this.closeDelete()
        },

        close () {
            this.dialog = false
            this.$nextTick(() => {
                this.editedItem = Object.assign({}, this.defaultItem)
                this.editedIndex = -1
            })
        },

        closeDelete () {
            this.dialogDelete = false
            this.$nextTick(() => {
                this.editedItem = Object.assign({}, this.defaultItem)
                this.editedIndex = -1
            })
        },

        save () {
            if (this.editedIndex > -1) {
                var data = JSON.parse(JSON.stringfy(this.editedItem));
                axios.put("/api/roadcategories/" + this.editedItem.category_road_id, data)
                .then(data => {
                    Object.assign(this.roadcategories[this.editedIndex], this.editedItem)
                }).catch(e=> { alert(e)})
               
            } else {
                var data = JSON.parse(JSON.stringfy(this.editedItem));
                axios.post("/api/roadcategories/" + this.editedItem.category_road_id, data)
                .then(data => {
                    this.roadcategories.push(this.editedItem)
                }).catch(e=> { alert(e) })
               
            }
            this.close()
        },
    },
})