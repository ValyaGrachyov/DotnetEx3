import { useEffect, useRef, useState } from "react";

export default function InfinityScroll({mapDisplayData, demandAdditionData, elementsPerPage}) {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [fetching, setFetching] = useState(true);

    const scrollHandler = (e) => {
        const targetDocumentElement = e.target.documentElement;
        console.log(targetDocumentElement.scrollHeight - (targetDocumentElement.scrollTop + window.innerHeight));
        if (targetDocumentElement.scrollHeight - (targetDocumentElement.scrollTop + window.innerHeight) < 100) {
                setFetching(true);
            }
    }

    useEffect(() => {
        if (fetching) {
            demandAdditionData(currentPage, elementsPerPage)
            .then(response => {
                setData(prev => [...prev, ...response.newData]);
                setCurrentPage(prev => prev + 1);
            })
            .finally(() => setFetching(false));
        }
    }, [fetching]);

    useEffect(() => {
        document.addEventListener("scroll", scrollHandler);

        return function () {
            document.removeEventListener("scroll", scrollHandler);
        };
    }, []);

    return <div className="list">
        {data.map((elem, index) => mapDisplayData(elem, index))}
    </div>
}