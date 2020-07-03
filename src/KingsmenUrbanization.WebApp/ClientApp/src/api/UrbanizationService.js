export default class UrbanizationService {
    async getTotalUrbanzationByStateDataCount() {
        try {
            const response = await fetch('urbanization/count');
            if (response.status === 200) {
                const data = await response.json();
                return data;
            } else {
                throw Error("Issue occurred when attempting to get total data rows");
            }
        } catch (e) {
            throw e;
        }
    }

    async getUrbanizationByStateData(page, rowsPerPage, orderBy, order){
        try {
            const response = await fetch('urbanization?' + createQueryString(page, rowsPerPage, orderBy, order), { 
                headers: {'Content-Type': 'application/json'}});
            if (response.status === 200) {
                const data = await response.json();
                return data;
            } else {
                const errorMessage = await response.json();
                throw errorMessage
            }
        } catch (e) {
            throw e;
        }
    }
}


function createQueryString(page, rowsPerPage, orderBy, order) {
    const data = {'page': page, 'rowsPerPage': rowsPerPage,
        'orderBy': orderBy ?? "", 'order': order ?? ""
    };
    return encodeData(data);
}

function encodeData (data) {
    const ret = [];
    for (let d in data)
      ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
    return ret.join('&');
}