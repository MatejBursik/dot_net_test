import os
import streamlit as st
import pandas as pd
from sqlalchemy import create_engine, text

DB_HOST = os.getenv("MYSQL_DB_HOST")
DB_USER = os.getenv("MYSQL_DB_USER")
DB_PASSWORD = os.getenv("MYSQL_DB_PASS")
DB_NAME = os.getenv("MYSQL_DB_NAME")

engine = create_engine(f"mysql+pymysql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}/{DB_NAME}")

st.title("Library DB Viewer")

if st.button("Refresh data"):
    try:
        with engine.connect() as connection:
            tables = pd.read_sql(text("SHOW TABLES"), connection)

            st.subheader("Tables")
            st.dataframe(tables, use_container_width=True)

            for table in tables.iloc[:, 0]:
                st.subheader(table)

                df = pd.read_sql(text(f"SELECT * FROM `{table}`"), connection)

                st.dataframe(df, use_container_width=True)

    except Exception as e:
        st.error(f"Database error: {e}")
