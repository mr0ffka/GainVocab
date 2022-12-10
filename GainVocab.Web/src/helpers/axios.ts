export const toURLSearchParams = (record: Record<string, unknown>) =>
  new URLSearchParams(
    Object.entries(record).reduce<string[][]>((result, [key, value]) => {
      if (Array.isArray(value)) {
        value.forEach((element) => result.push([key, String(element)]));
      } else {
        result.push([key, String(value)]);
      }
      return result;
    }, [])
  );
