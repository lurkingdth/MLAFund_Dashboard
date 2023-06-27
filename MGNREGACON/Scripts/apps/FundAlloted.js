

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes: false,

        fundallotment: {
            fundallotmentid: null,
            assembly_name: null,
            mla_name: null,
            allotted_fund: null,
            total_expenditure: null,
            financial_year: null,
            remarks: null,
        },

        assemblyList: null,
        mlaNameList:[],
        busy: false,
    },
    mounted(){
        axios.all([
            axios.get("/api/common/GetAssemblyList"),

        ]).then(axios.spread((data1) => {          
            this.assemblyList = data1.data;

        }))
       .catch(error => alert(error))
       .finally(() => {                
       });
    },


    methods: {
        FillMlaName() {
            axios.get('/api/Common/FillMlaName?assemblyName=' + this.fundallotment.assembly_name).then(data => {
                this.mlaNameList = data.data;
               
            })
        },

        saveData() {
            this.$validator.validate().then(valid => {
                if (valid) {
                    if (this.busy == true) return;
                    this.busy = true;
                    this.fundallotment.total_expenditure = 0;
                    var data = JSON.parse(JSON.stringify(this.fundallotment))
                    console.log(data)
                    axios.post("/api/FundAllotment/Save", data).then(d => {
                        if (d.data.status == "OK") {
                            alert("Data Sucessfully Saved")
                            window.location.href = '/FundAllotment/Create'
                        } else {
                            alert("Data Sucessfully Saved" + data)
                            alert(d.data.Message)
                        }


                    })
                } else {
                    this.busy = false;
                    alert(data1.data.Message)
                }
            }).finally(_ => {

            })
        },

        //Nishant Added code for Getting User data -start
        getUserDetailsMethod() {
            axios.get('/api/Common/UserDetails').then(data => {
                this.getUserDetails = data.data;
            })
        },
        //Nishant Added code for Getting User data -end

    }
})