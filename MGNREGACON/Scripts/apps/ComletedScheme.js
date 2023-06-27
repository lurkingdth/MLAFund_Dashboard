

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes:false,
        oldscheme :{
            
            oldscheme_id:null,
            division:null,
            subdivision:null,
            district:null,
            block:null,
            panchayat:null,
            village:null,
            recommendedby:null,
            mlampname:null,
            nameofconsti:null,
            sanction_year:null,
            schemename: null,
            category_road:null,
            category_road_detail: null,
            name_village_aspercategory:null,
            technologyused:null,
            administrativesanctionamount:null,
            refadministrativeapproval:null,
            technicalsanctionamount:null,
            contractorname: null,
            lengthofroad:null,
            regidcontractor:null,
            dateofagreement:null,
            agreementnumber:null,
            agreementamount:null,
            dateofcompletionasperagreement:null,
            dateofcompletionactual:null,
            totalexpenditure:null,
            filename:null,
            remark:null,
            usera:null,
            userb:null,
            userc:null,
            userd:null,
            submittedby:null,
            submitdate:null,
            status: "Completed",
            repairs:[],


        },
        months:["APRIL","MAY","JUNE","JULY","AUGUST","SEPTEMBER","OCTOBER","NOVEMBER","DECEMBER","JANUARY","FEBRUARY"],
        typelist:["Repair","Stregthning"],
        divisionlist:null,
        districtList:null,
        blocklist:[],
        panchayatList:[],
        villageList: [],
        busy: false,
        subdivisionList: null,
        category_road_detailList: null,
        assemblylist:null,
        roadcategorylist:null
    },
    mounted(){
        axios.all([
                  
        axios.get("/api/common/Division"),
        axios.get("/api/common/District"),
        axios.get("/api/common/assembly"),
         axios.get("/api/common/category_road"),

        ]).then(axios.spread((data1, data2,data3,data4) => {
            this.divisionlist = data1.data;
            this.districtList = data2.data;
            this.assemblylist = data3.data;
            this.roadcategorylist = data4.data;
        }))
       .catch(error => alert(error+"api fetch"))
       .finally(() => {                
       });
    },
    methods: {
        FillRoadcategorydetails() {
            
            axios.get('/api/common/category_road_detail?category_road=' + this.oldscheme.category_road).then(data => {
                this.category_road_detailList = data.data;
            })
        },
        FillSubdivision() {
            axios.get('/api/Common/subdivision?division=' + this.oldscheme.division).then(data => {
                this.subdivisionList = data.data;
            })
        },
        FillBlock(){
            axios.get('/api/Common/Block?district='+this.oldscheme.district).then(data => {
                this.blocklist=data.data;
            })
        },
        FillPanchayat(){
            axios.get('/api/common/panchayat?district='+this.oldscheme.district+'&block='+this.oldscheme.block).then(data => {
                this.panchayatList=data.data;
            })
        },
        FillVillage(){
            axios.get('/api/common/village?district='+this.oldscheme.district+'&block='+this.oldscheme.block+'&panchayat='+this.oldscheme.panchayat).then(data => {
                this.villageList=data.data;
            })

        },
        addRepair(){
            this.oldscheme.repairs.push( {
                repairs_id:null,
                oldscheme_id:null,
                schemename_r:null,
                sanction_year_r:null,
                type_r:null,
                lengthofroad_r: null, 
                technicalsanctionamount_r: null,
                administrativesanctionamount_r:null,
                referenceadministrativeapproval_r:null,
                contractorname_r:null,
                regidcontractor_r:null,
                agreementamount_r:null,
                agreementnumber_r:null,
                dateofagreement_r:null,
                dateofcompletionasperagreement_r:null,
                dateofcompletionactual_r:null,
                physicalpercent_r:null,
                totalexpenditure_r:null,
                filename_r:null,
                remark_r:null,
                usera_r:null,
                userb_r:null,
                userc_r:null,
                userd_r:null,
                submittedby_r:null,
                submitdate_r:null,
                status_r:"Completed",
                statusremark_r:null,


            });
        },
        remove(index){
            this.oldscheme.repairs.splice(index,1)
        },
        saveData() {

            this.$validator.validate().then(valid => {
                if (valid) {
                    if (this.busy == true) return;
                    this.busy = true;

                    /***f */
                    var formData = new FormData();
                    var imagefile = document.querySelector('#filename_c');
                    formData.append("txtname", imagefile.files[0]);
                    axios.post('/CompletedScheme/UploadFiles', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then(data1 => {
                        if (data1.data.status == "OK") {
                            this.oldscheme.filename = data1.data.file;
                            // alert(data1.data.file);
                            this.busy = false;
                            var data = JSON.parse(JSON.stringify(this.oldscheme))
                            console.log(data)
                            axios.post("/api/CompletedScheme/Save", data).then(d => {
                                if (d.data.status == "OK") {
                                    alert("Data saved")
                                    window.location.href = '/CompletedScheme/Create'
                                } else {
                                    alert(d.data.Message)
                                }


                            }).finally(_ => {
                                this.busy = false;

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
        rem(){
            this.oldscheme.repairs=[]
        }
    }




})