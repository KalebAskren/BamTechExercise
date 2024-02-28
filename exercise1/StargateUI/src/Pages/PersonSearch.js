import React, { useState } from 'react'
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import { MakeGetRequest } from '../Utility/Api';

export default function PersonSearch() {
    const[name, setName] = useState("");
    const[personInfo, setPersonInfo] = useState(null);
    function search() {
         MakeGetRequest('Person/' + name, null, (response) => {
            var value = JSON.parse(response);
            console.log(value);
            setPersonInfo(value.person)
         });
    }
  return (
    <div>
        <div>PersonSearch</div>
        <br/>
        <Box
            component="form"
            sx={{
                '& > :not(style)': { m: 1, width: '25ch' },
            }}
            noValidate
            autoComplete="off"
            >
            <TextField onChange={event => setName(event.target.value)} id="outlined-basic" label="Name" variant="outlined" />
            <Button onClick={x => search()} variant="contained">Search</Button>
            <br/>
            {
                personInfo? <>
                    Name: {personInfo.name}
                <br/>
                Rank: {personInfo.currentRank}
                <br/>
                Current Duty Title: {personInfo.currentDutyTitle}
                <br/>
                PersonID: {personInfo.personId}
                <br/>
                Career Start Date: {personInfo.careerStartDate}
                <br/>
                Career End Date: {personInfo.careerEndDate}
                </>
                :<></>
            }
        </Box>
    </div>
    
  )
}
