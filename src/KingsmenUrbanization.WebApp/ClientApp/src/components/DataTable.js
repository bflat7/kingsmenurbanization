import * as React from "react";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import TablePagination from "@material-ui/core/TablePagination";
import { TableSortLabel } from "@material-ui/core";
import Button from "@material-ui/core/Button";
import UrbanizationService from "../api/UrbanizationService";
import { makeStyles } from "@material-ui/core/styles";
import Backdrop from "@material-ui/core/Backdrop";
import CircularProgress from "@material-ui/core/CircularProgress";

const useStyles = makeStyles((theme) => ({
    backdrop: {
      zIndex: theme.zIndex.drawer + 1,
      color: "#fff",
    },
  }));

export const SortOrderEnum = {
    None: "none",
    Ascending: "asc",
    Descending: "desc",
}

export const TableHeaders = [
    {id: "stateFips", label: "State Fips", title: "Federal Information Processing"},
    {id: "stateName", label: "State", title: "State"},
    {id: "gisJoin", label: "GisJoin", title: "Geo-Information System"},
    {id: "latLong", label: "Lat/Long", title: "Geo-Coordinate System"},
    {id: "population", label: "Population", title: "Population in 2017"},
    {id: "urbanIndex", label: "Urban Index", title: "Urban Index"}
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
    const urbService = new UrbanizationService();
    const classes = useStyles();

    React.useEffect(() => {
        const getCount = async () => {
            try {
                const result = await urbService.getTotalUrbanzationByStateDataCount();
                setTotalCount(result);
            } catch (e) {
                Boolean(errorMessage) ? 
                    setError((prevError) => {prevError.concat("\n", e.message)}) :
                    setError(e.message);
            }
        }
        getCount();
    }, []);

    React.useEffect(() => {
        async function getData() {
            try {
                const result = await urbService.getUrbanizationByStateData(page, rowsPerPage, orderBy, order);
                setError(null);
                setUrbanizationData(result);
                setLoading(false);
            } catch (e) {
                Boolean(errorMessage) ? 
                    setError((prevError) => {prevError.concat("\n", e.message)}) :
                    setError(e.message);
                setUrbanizationData([]);
                setLoading(false);
            }
        }

        setLoading(true);
        getData();
    }, [page, rowsPerPage, orderBy, order]);

    const onHeaderClick = (event, headerId) => {
        if (headerId === orderBy)
            setOrder(order === SortOrderEnum.Ascending ? SortOrderEnum.Descending : SortOrderEnum.Ascending);
        else {
            setOrderBy(headerId);
            setOrder(SortOrderEnum.Ascending);
        } 
    }

    const getMuiDataTable = (dataSet) => {
        return (
            <div>
                {errorMessage && <><p aria-label={"Error Message"}>{errorMessage}</p><br/></>}
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                {TableHeaders.map((header) => {
                                    return (<TableCell key={header.id} aria-label={header.id} title={header.title} sortDirection={orderBy === header.id ? order : false}>
                                        <TableSortLabel active={header.id === orderBy} aria-label={"Sort Order"} direction={orderBy === header.id ? order : "asc"} onClick={(event) => onHeaderClick(event, header.id)}>{header.label}</TableSortLabel>
                                    </TableCell>)
                                })}
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {dataSet && dataSet.map((data) => (
                                <TableRow key={data.id}>
                                    <TableCell >{data.stateFips}</TableCell >
                                    <TableCell >{data.stateName}</TableCell >
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
    const handleClose = () => {
        setLoading(false);
    };
    
    const handleChangeRowsPerPage = (event) => {
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
            <Button variant="contained" onClick={(event) => {errorMessage ? setError(null) : setError("show error")}}>Show Error</Button>
                <Backdrop className={classes.backdrop} open={loading}>
                    <CircularProgress color="inherit" />
                </Backdrop>
        </div>
    )
}

