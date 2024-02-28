import * as React from 'react';
import { MakeGetRequest, MakePostRequest } from '../Utility/Api';
import { useEffect } from 'react';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';

const columns = [
    { field: 'id', headerName: 'ID', width: 90 },
    {
      field: 'name',
      headerName: 'Name',
      width: 150,
      editable: false,
    },
    {
      field: 'rank',
      headerName: 'Rank',
      width: 150,
      editable: false,
    },
    {
      field: 'dutyTitle',
      headerName: 'Duty Title',
      width: 110,
      editable: false,
    },
    {
      field: 'careerStartDate',
      headerName: 'Career Start Date',
      sortable: false,
      width: 250
    },
    {
        field: 'careerEndDate',
        headerName: 'Career End Date',
        sortable: false,
        width: 250
      },
  ];

export default function People() {
    const [rows, setRows] = React.useState([]);
    const [name, setName] = React.useState("");
    useEffect(() => {
        MakeGetRequest('Person/People', {} , (result) => parseRows(result))
        }, []);
    function parseRows(data){
        console.log(data);
        var people = JSON.parse(data).people;
        var tempRows = [];
        people.map(x => tempRows.push({id: x.personId, name: x.name, rank: x.currentRank, dutyTitle: x.currentDutyTitle, careerStartDate: x.careerStartDate, careerEndDate: x.careerEndDate}));
        setRows(tempRows);
    }
    function newPerson(){
        MakePostRequest('Person/'+ name, null, (result) => {
            alert(result);
        })
    }
    return (
        <div>All Astronauts
            <center>
                <Box sx={{ height: 400, width: '80%' }}>
                <DataGrid
                    rows={rows}
                    columns={columns}
                    initialState={{
                    pagination: {
                        paginationModel: {
                        pageSize: 5,
                        },
                    },
                    }}
                    pageSizeOptions={[5]}
                    disableRowSelectionOnClick
                />
                </Box>  
            </center>
            <br/>
            <center>
                Create New Person Entry
                <br></br>
                <TextField required onChange={event => setName(event.target.value)} id="outlined-basic" label="Name" variant="outlined" />
                <Button onClick={x => newPerson()} variant="contained">Submit</Button>

            </center>
        </div>
    )
}