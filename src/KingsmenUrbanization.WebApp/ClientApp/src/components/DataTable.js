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

const Headers = [
    {id: 'stateFips', label: 'State Fips'},
    {id: 'state', label: 'State'},
    {id: 'gisJoin', label: 'GisJoin'},
    {id: 'latLong', label: 'Lat/Long'},
    {id: 'population', label: 'Population'},
    {id: 'urbanIndex', label: 'Urban Index'}
];

export default function DataTable() {
    const [sortOrder, setSortOrder] = React.useState(SortOrderEnum.None);
    const [orderBy, setOrderBy] = React.useState();
    const [order, setOrder] = React.useState();
    const [page, setPage] = React.useState(0);
    const [urbanizationData, setUrbanizationData] = React.useState();
    const [totalCount, setTotalCount] = React.useState();
    const [rowsPerPage, setRowsPerPage] = React.useState(10);

    React.useEffect(() => {
        const getTotalUrbanizationByState = async () => {
            const response = await fetch('urbanization/count');
            const data = await response.json();
            setTotalCount(data);
        }
        getTotalUrbanizationByState();
    }, []);

    React.useEffect(() => {
        setUrbanizationData(undefined);
        const getUrbanizationByState = async () => {
            const response = await fetch('urbanization?' + createQueryString());
            const data = await response.json();
            setUrbanizationData(data);
        }
        getUrbanizationByState();
    }, [page, rowsPerPage, sortOrder]);

    const createQueryString = () => {
        const data = {'page': page, 'rowsPerPage': rowsPerPage,
        //  'orderBy': orderBy, 'order': order
        };
        return encodeData(data);
    }

    const encodeData = (data) => {
        const ret = [];
        for (let d in data)
          ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
        return ret.join('&');
    }

    const onHeaderClick = (event, headerId) => {
        if (headerId === orderBy)
            setOrder(SortOrderEnum.Descending);
        else {
            setOrderBy(headerId);
            setOrder(SortOrderEnum.Ascending);
        } 
    }

    const getMuiDataTable = (dataSet) => {
        return (
            <div>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                {Headers.map((header) => {
                                    return (<TableCell key={header.id} sortDirection={orderBy === header.id ? order : false}>
                                        <TableSortLabel active={header.id === orderBy} direction={orderBy === header.id ? order : 'asc'} onClick={(event) => onHeaderClick(event, header.id)}>{header.label}</TableSortLabel>
                                    </TableCell>)
                                })}
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {dataSet.map((data) => (
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
    
    const handleChangeRowsPerPage = (event, newPage) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    }
    const handlePageChange = (event, newPage) => {
        setPage(newPage);
    }

    return (
        <div>
            {urbanizationData ? 
                getMuiDataTable(urbanizationData) :
                <p><em>Loading...</em></p>
            }
        </div>
    )
}

