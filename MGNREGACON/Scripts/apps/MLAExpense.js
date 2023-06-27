

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes: false,

        mlaexpense: {
            expensesid: null,
            assembly_name: null,
            district:null,
            mla_name: null,
            expense_head: null,
            expense_detail: null,
            expense_amount: null,
            pending_fund: null,
            expense_doc: null,
            financial_year: null,

        },

        assemblyName: null,
        districtName: null,
        mlaName: null,
        financialYear:null,
        busy: false,
    },
    mounted() {
        this.financialYear = document.getElementById("financialYear").value;


        axios.all([
            axios.get("/api/common/GetAssemblyDetails"),

        ]).then(axios.spread((data1) => {
            this.assemblyName = data1.data[0].assembly_name
            this.mlaName = data1.data[0].mla_name
            this.districtName = data1.data[0].district

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
                    var formData = new FormData();
                    var imagefile = document.querySelector('#filename_c');
                    formData.append("txtname", imagefile.files[0]);
                    axios.post('/FundAllotment/UploadFiles', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then(data1 => {
                        if (data1.data.status == "OK") {
                            this.mlaexpense.expense_doc = data1.data.file;
                            this.mlaexpense.assembly_name = this.assemblyName;
                            this.mlaexpense.mla_name = this.mlaName;
                            this.mlaexpense.district = this.districtName;
                            this.mlaexpense.financial_year = this.financialYear;
                            this.busy = false;
                            var data = JSON.parse(JSON.stringify(this.mlaexpense))
                            console.log(data)
                            axios.post('/api/MLAExpense/Save', data).then(d => {
                                if (d.data.status == "OK") {
                                    alert("Data Sucessfully Saved")
                                    this.updateTotalExpenditure()
                                    window.location.href = '/Home/Index'
                                } else {
                                    alert(d.data.Message)
                                }
                            })
                        } else {
                            this.busy = false;
                            alert(data1.data.Message)
                        }
                    }).finally(_ => {

                    })
                }

            })
        },

        //Nishant Added code for Getting User data -start
        getUserDetailsMethod() {
            axios.get('/api/Common/UserDetails').then(data => {
                this.getUserDetails = data.data;
            })
        },
        //Nishant Added code for Getting User data -end

        //Nishant Added code for Updating the Total Expenditure on Fund Allocated table -start
        //This would pass on the Assembly Name and Financial Year from  CreateExpense()
        // to a new function which would SUM the expenses as per Financial Year and Assembly name
        // Once We get the total Expense from the MLAExpense table - we would then update the value to Fund Allocated table's total_expenditure

        updateTotalExpenditure() {
            $.post("/FundAllotment/updateTotalExpenditure", { financial_year: this.financialYear, assembly_name: this.assemblyName });
        }

        //Nishant Added code for Updating the Total Expenditure on Fund Allocated table -end

    }
})