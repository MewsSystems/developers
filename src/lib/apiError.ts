export class ApiError extends Error {
  public status: number;

  constructor(message: string, code: number) {
    super(message);
    this.status = code;

    Object.setPrototypeOf(this, new.target.prototype);

    this.name = this.constructor.name;
  }
}
