import { useEffect, useRef, useState } from "react";

export default function InfinityScroll(mapData, demandData, displayableElementsCount, onError) {
    const [currentPage, setCurrentPage] = useState(0);
    const [data, setData] = useState([]);
    const dataHolderRef = useRef(null);
    const [elementHeight, setElementHeight] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await demandData(currentPage, displayableElementsCount * 2);
                // todo: preprocess data and make sure there are requested data counts
                const data = response;
                setData(pre => [...pre, ...data]);
            }
            catch(err){
                onError(err);
            }
        }

        fetchData();
    }, [currentPage]);

    useEffect(() => {
        const handleScroll = (e) => {
            const scrollHeight = e.target.documentElement.scrollHeight;
            const currnetHeight = e.target.documentElement.scrollTop;
            if (elementHeight !== 0 && currnetHeight + elementHeight * displayableElementsCount >= scrollHeight)
                setCurrentPage(c => c + 1);
        }

        dataHolderRef.current?.addEventListener("scroll", handleScroll);

        return () => dataHolderRef.current.removeEventListener("scroll", handleScroll);
    }, [dataHolderRef, elementHeight]);

    useEffect(() => {
        if (elementHeight === 0 && dataHolderRef.current != null)
            setElementHeight(dataHolderRef.current.scrollHeight / 2.0 / displayableElementsCount);
    }, [dataHolderRef])

    return <div ref={dataHolderRef}>
        {data && data.map((elem, i) => mapData(elem, i))}
    </div>


}