

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes:false,
        completed_bridge: {
            bridge_completed_id: null,
            division:null,
            subdivision:null,
            district:null,
            block:null,
            panchayat:null,
            village:null,
            recommendedby:null,
            mlampname:null,
            nameofconsti:null,
            sanction_year: null,
            type_of_head:null,
            schemename: null,
            consultancy_name:null,
            type_of_foundation: null,
            amount_increase: null,
            amount_decrease: null,
            reasonofdeviation: null,
            sanction_aboutment: null,
            sanction_pier: null,
            sanction_slab: null,
            lengthof_approach_road: null,
            lengthof_retainingwall: null,
            administrativesanctionamount:null,
            refadministrativeapproval:null,
            technicalsanctionamount:null,
            contractorname:null,
            regidcontractor:null,
            dateofagreement:null,
            agreementnumber:null,
            agreementamount:null,
            dateofcompletionasperagreement:null,
            dateofcompletionactual:null,
            totalexpenditure: null,
            lengthofbridge: null,
            filename:null,
            remark:null,
            usera:null,
            userb:null,
            userc:null,
            userd:null,
            submittedby:null,
            submitdate:null,
            status:null,
            repairs:[],


        },
        months:["APRIL","MAY","JUNE","JULY","AUGUST","SEPTEMBER","OCTOBER","NOVEMBER","DECEMBER","JANUARY","FEBRUARY"],
        typelist:["Repair","Stregthning"],
        divisionlist:null,
        districtList:null,
        blocklist:[],
        panchayatList:[],
        villageList:[],
        busy: false,
        subdivisionList: null,
        assemblylist:null
    },
    mounted(){
        axios.all([
                  
        axios.get("/api/common/Divisionb"),
        axios.get("/api/common/District"),
        axios.get("/api/common/assembly"),

        ]).then(axios.spread((data1, data2,data3) => {
            this.divisionlist = data1.data;
            this.districtList = data2.data;
            this.assemblylist = data3.data;
        }))
       .catch(error => alert(error))
       .finally(() => {                
       });
    },
    methods: {
        FillSubdivision() {
            axios.get('/api/Common/subdivisionb?division=' + this.completed_bridge.division).then(data => {
                this.subdivisionList = data.data;
            })
        },
        FillBlock(){
            axios.get('/api/Common/Block?district=' + this.completed_bridge.district).then(data => {
                this.blocklist=data.data;
            })
        },
        FillPanchayat(){
            axios.get('/api/common/panchayat?district=' + this.completed_bridge.district + '&block=' + this.completed_bridge.block).then(data => {
                this.panchayatList=data.data;
            })
        },
        FillVillage(){
            axios.get('/api/common/village?district=' + this.completed_bridge.district + '&block=' + this.completed_bridge.block + '&panchayat=' + this.completed_bridge.panchayat).then(data => {
                this.villageList=data.data;
            })

        },
        saveData(){
            this.$validator.validate().then(valid => {
                if (valid) {
                    if (this.busy == true) return;
                    this.busy = true;
                    var formData = new FormData();
                    var imagefile = document.querySelector('#filename_c');
                    formData.append("txtname", imagefile.files[0]);
                    axios.post('/CompletedBridge/UploadFiles', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then(data1 => {
                        if (data1.data.status == "OK") {
                            this.completed_bridge.filename = data1.data.file;
                            // alert(data1.data.file);
                            this.busy = false;
                            var data = JSON.parse(JSON.stringify(this.completed_bridge))
                            console.log(data)
                            axios.post("/api/CompletedBridge/Save", data).then(d => {
                                if (d.data.status == "OK") {
                                    alert("Data Sucessfully Saved")
                                    window.location.href = '/CompletedBridge/Create'
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
    }
})