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
import LinearProgress from "@material-ui/core/LinearProgress";

const useStyles = makeStyles((theme) => ({
    backdrop: {
      zIndex: theme.zIndex.drawer + 1,
      color: "#fff",
    },
    tblContainer: {
      height: 440,
    },
    container: {
        marginTop: "3rem",
    },
    progress: {
        top: "50%",
    }
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
    const [errorMessage, setErrorMessage] = React.useState();
    const [loading, setLoading] = React.useState(false);
    const urbService = new UrbanizationService();
    const classes = useStyles();
    
    let controllerRef = React.useRef(new AbortController());

    React.useEffect(() => {
        const getCount = async () => {
            try {
                const result = await urbService.getTotalUrbanzationByStateDataCount();
                setTotalCount(result);
            } catch (e) {
                Boolean(errorMessage) ? 
                    setErrorMessage((prevError) => {prevError.concat("\n", e.message)}) :
                    setErrorMessage(e.message);
            }   
        }
        getCount();
    }, []);

    React.useEffect(() => {
        if (loading) {
            controllerRef.current.abort();
            controllerRef.current = new AbortController();
        }
        setLoading(true);
        
        urbService.getUrbanizationByStateData(page, rowsPerPage, orderBy, order, controllerRef.current.signal).then((data) => {
            setErrorMessage(null);
            setUrbanizationData(data);
            setLoading(false);
        }).catch((err) => {
            if (err.Name = "AbortError") {
                setErrorMessage(err.message);
            } else {
                Boolean(errorMessage) ? 
                    setErrorMessage((prevError) => {prevError.concat("\n", err.message)}) :
                    setErrorMessage(err.message);
                setUrbanizationData([]);
                setLoading(false);
            }
        });
    }, [page, rowsPerPage, orderBy, order]);

    const onHeaderClick = (event, headerId) => {
        if (headerId === orderBy)
            setOrder(order === SortOrderEnum.Ascending ? SortOrderEnum.Descending : SortOrderEnum.Ascending);
        else {
            setOrderBy(headerId);
            setOrder(SortOrderEnum.Ascending);
        }
        setUrbanizationData([]);
        setLoading(true);
    }

    const getMuiDataTable = (dataSet) => {
        return (
            <div className={classes.container}>
                {errorMessage && <><p aria-label={"Error Message"}>{errorMessage}</p><br/></>}
                <TableContainer component={Paper} className={classes.tblContainer}>
                    {loading ? <LinearProgress className={classes.progress} color="primary"/> :
                    <Table>
                        <TableHead>
                            <TableRow>
                                {TableHeaders.map((header) => {
                                    return (<TableCell key={header.id} align="center" aria-label={header.id} title={header.title} sortDirection={orderBy === header.id ? order : false}>
                                        <TableSortLabel active={header.id === orderBy} aria-label={"Sort Order"} direction={orderBy === header.id ? order : "asc"} onClick={(event) => onHeaderClick(event, header.id)}>{header.label}</TableSortLabel>
                                    </TableCell>)
                                })}
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {dataSet && dataSet.map((data) => (
                                <TableRow key={data.id}>
                                    <TableCell align="center">{data.stateFips}</TableCell >
                                    <TableCell align="center">{data.stateName}</TableCell >
                                    <TableCell align="center">{data.gisJoin}</TableCell >
                                    <TableCell align="center">{data.latLong}</TableCell >
                                    <TableCell align="center">{data.population}</TableCell >
                                    <TableCell align="center">{data.urbanIndex}</TableCell >
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>}
                </TableContainer>
                <TablePagination
                    rowsPerPageOptions={[10, 20, 50, 100, 10000]}
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
        setUrbanizationData([]);
    }
    const handlePageChange = (event, newPage) => {
        setPage(newPage);
        setUrbanizationData([]);
    }

    return (
        <div>
            {urbanizationData ?
                getMuiDataTable(urbanizationData) :
                <p><em>Loading...</em></p>
            }
            <Button disabled={!loading} variant="contained" color="primary" onClick={(event) => { 
                controllerRef.current.abort();
                controllerRef.current = new AbortController();
                setUrbanizationData([]);
                setLoading(false);}}> Abort Call </Button>
        </div>
    )
}

