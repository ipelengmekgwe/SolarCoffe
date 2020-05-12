export interface IServiceResponse<T> {
    isSuccess: boolean,
    time: Date,
    message: string,
    data: T
}
