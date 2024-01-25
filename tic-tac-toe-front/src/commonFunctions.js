export function isNullOrEmptyString (str) {
    return (typeof str === "string" && str.length === 0) || str === null;
}