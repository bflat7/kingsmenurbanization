import * as React from "react";
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import TablePagination from '@material-ui/core/TablePagination';
import { TableSortLabel } from "@material-ui/core";

const SortOrderEnum = {
    None: 'none',
    Ascending: 'asc',
    Descending: 'desc',
}

const TableHeaders = [
    {id: 'stateFips', label: 'State Fips'},
    {id: 'stateName', label: 'State'},
    {id: 'gisJoin', label: 'GisJoin'},
    {id: 'latLong', label: 'Lat/Long'},
    {id: 'population', label: 'Population'},
    {id: 'urbanIndex', label: 'Urban Index'}
];

export default function DataTable() {
    const [orderBy, setOrderBy] = React.useState();
    const [order, setOrder] = React.useState();
    const [page, setPage] = React.useState(0);
    const [urbanizationData, setUrbanizationData] = React.useState();
    const [totalCount, setTotalCount] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);
    const [errorMessage, setError] = React.useState();
    const [loading, setLoading] = React.useState(true);

    React.useEffect(() => {
        const getTotalUrbanizationByState = async () => {
            const response = await fetch('urbanization/count');
            const data = await response.json();
            setTotalCount(data);
        }
        getTotalUrbanizationByState();
    }, []);

    React.useEffect(() => {
        setLoading(true);
        
        const createQueryString = () => {
            const data = {'page': page, 'rowsPerPage': rowsPerPage,
             'orderBy': orderBy ?? "", 'order': order ?? ""
            };
            return encodeData(data);
        }

        const getUrbanizationByState = async () => {
            // fetch('urbanization?' + createQueryString()).then((resp) => resp.json()).then((data) => setUrbanizationData(data));
            const myHeaders = new Headers();
            myHeaders.append('Content-Type', 'application/json');
            const response = await fetch('urbanization?' + createQueryString(), { 
                headers: {'Content-Type': 'application/json'}});
            if (response.status == 200) {
                const data = await response.json();
                setUrbanizationData(data);
            } else {
                const errorMessage = await response.json();
                setUrbanizationData([]);
                setError(errorMessage);
            }
            setLoading(false);
        }
        getUrbanizationByState();
    }, [page, rowsPerPage, orderBy, order]);


    const encodeData = (data) => {
        const ret = [];
        for (let d in data)
          ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
        return ret.join('&');
    }

    const onHeaderClick = (event, headerId) => {
        if (headerId === orderBy)
            setOrder(order === SortOrderEnum.Ascending ? SortOrderEnum.Descending : SortOrderEnum.Ascending);
        else {
            setOrderBy(headerId);
            setOrder(SortOrderEnum.Ascending);
        } 
    }

    const getMuiDataTable = (dataSet) => {
        console.log(loading);
        return (
            <div>
                {errorMessage && <><p>{errorMessage}</p><br/></>}
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                {TableHeaders.map((header) => {
                                    return (<TableCell key={header.id} sortDirection={orderBy === header.id ? order : false}>
                                        <TableSortLabel active={header.id === orderBy} direction={orderBy === header.id ? order : 'asc'} onClick={(event) => onHeaderClick(event, header.id)}>{header.label}</TableSortLabel>
                                    </TableCell>)
                                })}
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {dataSet && dataSet.map((data) => (
                                <TableRow key={data.id}>
                                    <TableCell >{data.stateName}</TableCell >
                                    <TableCell >{data.stateFips}</TableCell >
                                    <TableCell >{data.gisJoin}</TableCell >
                                    <TableCell >{data.latLong}</TableCell >
                                    <TableCell >{data.population}</TableCell >
                                    <TableCell >{data.urbanIndex}</TableCell >
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
                <TablePagination
                    rowsPerPageOptions={[10, 20, 50, 10000]}
                    component="div"
                    count={totalCount}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    onChangePage={handlePageChange}
                    onChangeRowsPerPage={handleChangeRowsPerPage}/>
            </div>
        )
    }
    
    const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    }
    const handlePageChange = (event, newPage) => {
        setPage(newPage);
    }

    return (
        <div>
            {!loading ? 
                getMuiDataTable(urbanizationData) :
                <p><em>Loading...</em></p>
            }
        </div>
    )
}

